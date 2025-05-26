using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly AppDbContext _context;

        public ManufacturerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Manufacturer> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Manufacturers
                .Include(m => m.Vehicles)
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }
        public async Task<IEnumerable<Manufacturer>> GetAllAsync(CancellationToken cancellationToken)
        {
            var result = await _context.Manufacturers
                .ToListAsync(cancellationToken);
            return result;
        }
        public async Task AddAsync(Manufacturer manufacturer, CancellationToken cancellationToken)
        {
            await _context.Manufacturers.AddAsync(manufacturer, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Manufacturer manufacturer, CancellationToken cancellationToken)
        {
            _context.Manufacturers.Update(manufacturer);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var manufacturer = await GetByIdAsync(id, cancellationToken);
            if (manufacturer != null)
            {
                _context.Manufacturers.Remove(manufacturer);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        public async Task<Manufacturer> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Manufacturers
                .Include(m => m.Vehicles)
                .FirstOrDefaultAsync(m => m.Name == name, cancellationToken);
        }
        public async Task<bool> ExistsAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Manufacturers.AnyAsync(m => m.Name == name, cancellationToken);
        }
    }
}
