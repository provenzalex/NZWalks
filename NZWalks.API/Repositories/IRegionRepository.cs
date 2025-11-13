using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepository
    {
        Task<Region> CreateAsync(Region region);

        Task<Region?> DeleteAsync(Guid id);

        Task<Region?> GetByIdAsync(Guid id);

        Task<List<Region>> GetAllAsync();

        Task<Region?> UpdateAsync(Guid id, Region region);
    }
}
