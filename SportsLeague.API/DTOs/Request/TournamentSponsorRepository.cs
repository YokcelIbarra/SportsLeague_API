using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories
{
    public class TournamentSponsorRepository : ITournamentSponsorRepository
    {
        private readonly LeagueDbContext _context;

        public TournamentSponsorRepository(LeagueDbContext context)
        {
            _context = context;
        }

        public async Task<TournamentSponsor> CreateAsync(TournamentSponsor tournamentSponsor)
        {
            await _context.TournamentSponsors.AddAsync(tournamentSponsor);
            await _context.SaveChangesAsync();
            return tournamentSponsor;
        }

        public async Task<IEnumerable<TournamentSponsor>> GetBySponsorIdAsync(int sponsorId)
        {
            return await _context.TournamentSponsors
                .Include(ts => ts.Tournament)
                .Include(ts => ts.Sponsor)
                .Where(ts => ts.SponsorId == sponsorId)
                .ToListAsync();
        }

        public async Task<TournamentSponsor?> GetBySponsorAndTournamentAsync(int sponsorId, int tournamentId)
        {
            return await _context.TournamentSponsors
                .Include(ts => ts.Tournament)
                .Include(ts => ts.Sponsor)
                .FirstOrDefaultAsync(ts => ts.SponsorId == sponsorId && ts.TournamentId == tournamentId);
        }

        public async Task DeleteAsync(TournamentSponsor tournamentSponsor)
        {
            _context.TournamentSponsors.Remove(tournamentSponsor);
            await _context.SaveChangesAsync();
        }
    }
}