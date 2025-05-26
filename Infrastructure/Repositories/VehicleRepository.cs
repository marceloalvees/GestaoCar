using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext _context;
        public VehicleRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Vehicles
                .Include(v => v.Manufacturer)
                .ToListAsync();
        }
        public async Task<Vehicle> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Vehicles.FindAsync(id);
        }
        public async Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken)
        {
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Vehicle vehicle, CancellationToken cancellationToken)
        {
            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var vehicle = await GetByIdAsync(id, cancellationToken);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Vehicle> GetByModelAsync(string model, CancellationToken cancellationToken)
        {

            return await _context.Vehicles
                .FirstOrDefaultAsync(
                    v => v.Model.ToUpper() == model.ToUpper(),
                    cancellationToken
                );
        }
    }
}
