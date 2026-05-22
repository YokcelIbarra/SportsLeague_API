using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories
{
    public class TournamentSponsorRepository : GenericRepository<TournamentSponsor>, ITournamentSponsorRepository
    {
        public TournamentSponsorRepository(LeagueDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TournamentSponsor>> GetBySponsorIdAsync(int sponsorId)
        {
            return await _dbSet
                .Include(ts => ts.Tournament)
                .Include(ts => ts.Sponsor)
                .Where(ts => ts.SponsorId == sponsorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<TournamentSponsor>> GetByTournamentIdAsync(int tournamentId)
        {
            return await _dbSet
                .Include(ts => ts.Tournament)
                .Include(ts => ts.Sponsor)
                .Where(ts => ts.TournamentId == tournamentId)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int sponsorId, int tournamentId)
        {
            return await _dbSet
                .AnyAsync(ts => ts.SponsorId == sponsorId && ts.TournamentId == tournamentId);
        }

        public async Task<TournamentSponsor?> GetBySponsorAndTournamentAsync(int sponsorId, int tournamentId)
        {
            return await _dbSet
                .Include(ts => ts.Tournament)
                .Include(ts => ts.Sponsor)
                .FirstOrDefaultAsync(ts => ts.SponsorId == sponsorId && ts.TournamentId == tournamentId);
        }

        public async Task DeleteAsync(TournamentSponsor tournamentSponsor)
        {
            _dbSet.Remove(tournamentSponsor);
            await _context.SaveChangesAsync();
        }
    }
}
