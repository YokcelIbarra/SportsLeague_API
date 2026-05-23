using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Services
{
    public interface IMatchLineupService
    {
        Task<MatchLineup> CreateAsync(int matchId, MatchLineup matchLineup);
        Task<IEnumerable<MatchLineup>> GetByMatchAsync(int matchId);
        Task<IEnumerable<MatchLineup>> GetByMatchAndTeamAsync(int matchId, int teamId);
        Task DeleteAsync(int matchId, int id);
    }
}
