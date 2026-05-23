using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;

namespace SportsLeague.Domain.Interfaces.Services
{
    public interface IMatchService
    {
        Task<IEnumerable<Match>> GetAllAsync();
        Task<Match?> GetByIdAsync(int id);
        Task<IEnumerable<Match>> GetByTournamentAsync(int tournamentId);
        Task<Match> CreateAsync(Match match);
        Task UpdateAsync(int id, Match match);
        Task DeleteAsync(int id);
        Task UpdateStatusAsync(int id, MatchStatus newStatus);
    }
}
