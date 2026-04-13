using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface ITournamentSponsorRepository
    {
        Task<TournamentSponsor> CreateAsync(TournamentSponsor tournamentSponsor);
        Task<IEnumerable<TournamentSponsor>> GetBySponsorIdAsync(int sponsorId);
        Task<TournamentSponsor?> GetBySponsorAndTournamentAsync(int sponsorId, int tournamentId);
        Task DeleteAsync(TournamentSponsor tournamentSponsor);
    }
}