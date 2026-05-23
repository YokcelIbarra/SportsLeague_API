using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services
{
    public class MatchLineupService : IMatchLineupService
    {
        private readonly IMatchLineupRepository _matchLineupRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ILogger<MatchLineupService> _logger;

        public MatchLineupService(
            IMatchLineupRepository matchLineupRepository,
            IMatchRepository matchRepository,
            IPlayerRepository playerRepository,
            ITeamRepository teamRepository,
            ILogger<MatchLineupService> logger)
        {
            _matchLineupRepository = matchLineupRepository;
            _matchRepository = matchRepository;
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
            _logger = logger;
        }

        public async Task<MatchLineup> CreateAsync(int matchId, MatchLineup matchLineup)
        {
            var match = await GetScheduledMatchAsync(matchId);
            var player = await _playerRepository.GetByIdWithTeamAsync(matchLineup.PlayerId);

            if (player == null)
                throw new KeyNotFoundException($"No se encontró el jugador con ID {matchLineup.PlayerId}");

            if (player.TeamId != match.HomeTeamId && player.TeamId != match.AwayTeamId)
                throw new InvalidOperationException("El jugador no pertenece a los equipos del partido");

            var existing = await _matchLineupRepository
                .GetByMatchAndPlayerAsync(matchId, matchLineup.PlayerId);

            if (existing != null)
                throw new InvalidOperationException("El jugador ya está registrado en la alineación del partido");

            if (matchLineup.IsStarter)
            {
                var startersCount = await _matchLineupRepository
                    .CountStartersByMatchAndTeamAsync(matchId, player.TeamId);

                if (startersCount >= 11)
                    throw new InvalidOperationException("Un equipo no puede tener más de 11 titulares por partido");
            }

            matchLineup.MatchId = matchId;

            _logger.LogInformation(
                "Registering player {PlayerId} in lineup for match {MatchId}",
                matchLineup.PlayerId, matchId);

            var created = await _matchLineupRepository.CreateAsync(matchLineup);
            return await _matchLineupRepository.GetByIdAndMatchAsync(created.Id, matchId) ?? created;
        }

        public async Task<IEnumerable<MatchLineup>> GetByMatchAsync(int matchId)
        {
            await EnsureMatchExistsAsync(matchId);
            return await _matchLineupRepository.GetByMatchAsync(matchId);
        }

        public async Task<IEnumerable<MatchLineup>> GetByMatchAndTeamAsync(int matchId, int teamId)
        {
            var match = await EnsureMatchExistsAsync(matchId);
            var teamExists = await _teamRepository.ExistsAsync(teamId);

            if (!teamExists)
                throw new KeyNotFoundException($"No se encontró el equipo con ID {teamId}");

            if (teamId != match.HomeTeamId && teamId != match.AwayTeamId)
                throw new InvalidOperationException("El equipo no pertenece al partido");

            return await _matchLineupRepository.GetByMatchAndTeamAsync(matchId, teamId);
        }

        public async Task DeleteAsync(int matchId, int id)
        {
            await GetScheduledMatchAsync(matchId);

            var matchLineup = await _matchLineupRepository.GetByIdAndMatchAsync(id, matchId);

            if (matchLineup == null)
                throw new KeyNotFoundException($"No se encontró la alineación con ID {id}");

            _logger.LogInformation("Deleting lineup {LineupId} from match {MatchId}", id, matchId);
            await _matchLineupRepository.DeleteAsync(id);
        }

        private async Task<Match> EnsureMatchExistsAsync(int matchId)
        {
            var match = await _matchRepository.GetByIdAsync(matchId);

            if (match == null)
                throw new KeyNotFoundException($"No se encontró el partido con ID {matchId}");

            return match;
        }

        private async Task<Match> GetScheduledMatchAsync(int matchId)
        {
            var match = await EnsureMatchExistsAsync(matchId);

            if (match.Status != MatchStatus.Scheduled)
                throw new InvalidOperationException("Solo se pueden registrar alineaciones si el partido está en estado Scheduled");

            return match;
        }
    }
}
