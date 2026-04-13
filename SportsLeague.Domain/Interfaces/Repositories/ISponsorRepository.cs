using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface ISponsorRepository
    {
        Task<IEnumerable<Sponsor>> GetAllAsync();
        Task<Sponsor?> GetByIdAsync(int id);
        Task<Sponsor> CreateAsync(Sponsor sponsor);
        Task UpdateAsync(Sponsor sponsor);
        Task DeleteAsync(Sponsor sponsor);
        Task<bool> ExistsByNameAsync(string name);
    }
}