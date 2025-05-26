using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IVehicleRepository
    {
        Task<Vehicle> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken);
        Task UpdateAsync(Vehicle vehicle, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        Task<Vehicle> GetByModelAsync(string model, CancellationToken cancellationToken);
    }
}
