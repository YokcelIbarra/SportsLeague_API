using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface IMatchRepository : IGenericRepository<Match>
    {
        Task<IEnumerable<Match>> GetAllWithDetailsAsync();
        Task<Match?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Match>> GetByTournamentAsync(int tournamentId);
        Task<IEnumerable<Match>> GetByStatusAsync(MatchStatus status);
    }
}
