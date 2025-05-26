using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IDealershipRepository
    {
        Task<Dealership> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Dealership>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(Dealership dealership, CancellationToken cancellationToken);
        Task UpdateAsync(Dealership dealership, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        Task<Dealership> GetByNameAsync(string name, CancellationToken cancellationToken);
    }
}
