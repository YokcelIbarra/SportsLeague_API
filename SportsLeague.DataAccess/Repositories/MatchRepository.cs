using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories
{
    public class MatchRepository : GenericRepository<Match>, IMatchRepository
    {
        public MatchRepository(LeagueDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Match>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(m => m.Tournament)
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Include(m => m.Referee)
                .ToListAsync();
        }

        public async Task<Match?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Where(m => m.Id == id)
                .Include(m => m.Tournament)
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Include(m => m.Referee)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Match>> GetByTournamentAsync(int tournamentId)
        {
            return await _dbSet
                .Where(m => m.TournamentId == tournamentId)
                .Include(m => m.Tournament)
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Include(m => m.Referee)
                .ToListAsync();
        }

        public async Task<IEnumerable<Match>> GetByStatusAsync(MatchStatus status)
        {
            return await _dbSet
                .Where(m => m.Status == status)
                .Include(m => m.Tournament)
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Include(m => m.Referee)
                .ToListAsync();
        }
    }
}
