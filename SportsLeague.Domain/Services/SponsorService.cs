using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services
{
    public class SponsorService : ISponsorService
    {
        private readonly ISponsorRepository _repository;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly ITournamentSponsorRepository _tournamentSponsorRepository;

        public SponsorService(
            ISponsorRepository repository,
            ITournamentRepository tournamentRepository,
            ITournamentSponsorRepository tournamentSponsorRepository)
        {
            _repository = repository;
            _tournamentRepository = tournamentRepository;
            _tournamentSponsorRepository = tournamentSponsorRepository;
        }

        public async Task<IEnumerable<Sponsor>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Sponsor?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Sponsor> CreateAsync(Sponsor sponsor)
        {
            if (await _repository.ExistsByNameAsync(sponsor.Name))
                throw new InvalidOperationException("Ya existe un sponsor con ese nombre");

            sponsor.CreatedAt = DateTime.UtcNow;

            return await _repository.CreateAsync(sponsor);
        }

        public async Task UpdateAsync(int id, Sponsor sponsor)
        {
            var existing = await _repository.GetByIdAsync(id);

            if (existing == null)
                throw new KeyNotFoundException("Sponsor no encontrado");

            existing.Name = sponsor.Name;
            existing.ContactEmail = sponsor.ContactEmail;
            existing.Phone = sponsor.Phone;
            existing.WebsiteUrl = sponsor.WebsiteUrl;
            existing.Category = sponsor.Category;
            existing.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);

            if (existing == null)
                throw new KeyNotFoundException("Sponsor no encontrado");

            await _repository.DeleteAsync(existing);
        }

        // 🔥 LINK
        public async Task<TournamentSponsor> LinkTournamentAsync(int sponsorId, int tournamentId, decimal contractAmount)
        {
            var sponsor = await _repository.GetByIdAsync(sponsorId);
            if (sponsor == null)
                throw new KeyNotFoundException("Sponsor no encontrado");

            var tournament = await _tournamentRepository.GetByIdAsync(tournamentId);
            if (tournament == null)
                throw new KeyNotFoundException("Tournament no encontrado");

            if (contractAmount <= 0)
                throw new InvalidOperationException("ContractAmount debe ser mayor a 0");

            var exists = await _tournamentSponsorRepository.GetBySponsorAndTournamentAsync(sponsorId, tournamentId);
            if (exists != null)
                throw new InvalidOperationException("Ya está vinculado");

            var ts = new TournamentSponsor
            {
                SponsorId = sponsorId,
                TournamentId = tournamentId,
                ContractAmount = contractAmount,
                JoinedAt = DateTime.UtcNow
            };

            return await _tournamentSponsorRepository.CreateAsync(ts);
        }

        // 🔥 GET TORNEOS
        public async Task<IEnumerable<TournamentSponsor>> GetTournamentsBySponsorAsync(int sponsorId)
        {
            return await _tournamentSponsorRepository.GetBySponsorIdAsync(sponsorId);
        }

        // 🔥 UNLINK
        public async Task UnlinkTournamentAsync(int sponsorId, int tournamentId)
        {
            var ts = await _tournamentSponsorRepository
                .GetBySponsorAndTournamentAsync(sponsorId, tournamentId);

            if (ts == null)
                throw new KeyNotFoundException("No existe vínculo");

            await _tournamentSponsorRepository.DeleteAsync(ts);
        }
    }
}