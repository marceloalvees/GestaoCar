using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IManufacturerRepository
    {
        Task<Manufacturer> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Manufacturer>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(Manufacturer manufacturer, CancellationToken cancellationToken);
        Task UpdateAsync(Manufacturer manufacturer, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(string name, CancellationToken cancellationToken);
        Task<Manufacturer> GetByNameAsync(string name, CancellationToken cancellationToken);
    }
}
