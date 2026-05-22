using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;
using System.Net.Mail;

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
            ValidateContactEmail(sponsor.ContactEmail);

            if (await _repository.ExistsByNameAsync(sponsor.Name))
                throw new InvalidOperationException("Ya existe un sponsor con ese nombre");

            sponsor.CreatedAt = DateTime.UtcNow;
            sponsor.UpdatedAt = null;

            return await _repository.CreateAsync(sponsor);
        }

        public async Task UpdateAsync(int id, Sponsor sponsor)
        {
            var existing = await _repository.GetByIdAsync(id);

            if (existing == null)
                throw new KeyNotFoundException("Sponsor no encontrado");

            ValidateContactEmail(sponsor.ContactEmail);

            if (await _repository.ExistsByNameAsync(sponsor.Name, id))
                throw new InvalidOperationException("Ya existe un sponsor con ese nombre");

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

            var exists = await _tournamentSponsorRepository.ExistsAsync(sponsorId, tournamentId);
            if (exists)
                throw new InvalidOperationException("Ya esta vinculado");

            var tournamentSponsor = new TournamentSponsor
            {
                SponsorId = sponsorId,
                TournamentId = tournamentId,
                ContractAmount = contractAmount,
                JoinedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            return await _tournamentSponsorRepository.CreateAsync(tournamentSponsor);
        }

        public async Task<IEnumerable<TournamentSponsor>> GetTournamentsBySponsorAsync(int sponsorId)
        {
            var sponsor = await _repository.GetByIdAsync(sponsorId);

            if (sponsor == null)
                throw new KeyNotFoundException("Sponsor no encontrado");

            return await _tournamentSponsorRepository.GetBySponsorIdAsync(sponsorId);
        }

        public async Task UnlinkTournamentAsync(int sponsorId, int tournamentId)
        {
            var sponsor = await _repository.GetByIdAsync(sponsorId);
            if (sponsor == null)
                throw new KeyNotFoundException("Sponsor no encontrado");

            var tournament = await _tournamentRepository.GetByIdAsync(tournamentId);
            if (tournament == null)
                throw new KeyNotFoundException("Tournament no encontrado");

            var tournamentSponsor = await _tournamentSponsorRepository
                .GetBySponsorAndTournamentAsync(sponsorId, tournamentId);

            if (tournamentSponsor == null)
                throw new KeyNotFoundException("No existe vinculo");

            await _tournamentSponsorRepository.DeleteAsync(tournamentSponsor);
        }

        private static void ValidateContactEmail(string contactEmail)
        {
            try
            {
                var email = new MailAddress(contactEmail);

                if (email.Address != contactEmail)
                    throw new InvalidOperationException("ContactEmail no tiene un formato valido");
            }
            catch (FormatException)
            {
                throw new InvalidOperationException("ContactEmail no tiene un formato valido");
            }
        }
    }
}
