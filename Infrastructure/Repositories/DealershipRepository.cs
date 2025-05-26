using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DealershipRepository : IDealershipRepository
    {
        private readonly AppDbContext _context;

        public DealershipRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Dealership>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Dealerships.ToListAsync(cancellationToken);
        }
        public async Task<Dealership> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Dealerships.FindAsync(new object[] { id }, cancellationToken);
        }
        public async Task AddAsync(Dealership dealership, CancellationToken cancellationToken)
        {
            await _context.Dealerships.AddAsync(dealership, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdateAsync(Dealership dealership, CancellationToken cancellationToken)
        {
            _context.Dealerships.Update(dealership);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var dealership = await GetByIdAsync(id, cancellationToken);
            if (dealership != null)
            {
                _context.Dealerships.Remove(dealership);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        public async Task<Dealership> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Dealerships
                    .FirstOrDefaultAsync(
                        d => d.Name.ToUpper() == name.ToUpper(),
                        cancellationToken
    );
        }
    }
}
