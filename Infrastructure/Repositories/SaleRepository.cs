using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly AppDbContext _context;
        public SaleRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Sale> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Sales
                .Include(s => s.Vehicle)
                .Include(s => s.Dealership)
                .Include(s => s.Client)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }
        public async Task<IEnumerable<Sale>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Sales
                .Include(s => s.Vehicle)
                .Include(s => s.Dealership)
                .Include(s => s.Client)
                .ToListAsync(cancellationToken);
        }
        public async Task AddAsync(Sale sale, CancellationToken cancellationToken)
        {
            await _context.Sales.AddAsync(sale, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdateAsync(Sale sale, CancellationToken cancellationToken)
        {
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var sale = await GetByIdAsync(id, cancellationToken);
            if (sale != null)
            {
                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        public async Task<IEnumerable<Sale>> GetSaleByMonthAndYearAsync(int month, int year, CancellationToken cancellationToken)
        {
            return await _context.Sales
                .Include(s => s.Vehicle)
                .Include(s => s.Dealership)
                .Include(s => s.Client)
                .Where(s => s.SaleDate.Month == month && s.SaleDate.Year == year)
                .ToListAsync(cancellationToken);
        }
    }
}
