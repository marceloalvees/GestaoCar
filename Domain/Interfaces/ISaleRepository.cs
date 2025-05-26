using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ISaleRepository
    {
        Task<Sale> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Sale>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(Sale sale, CancellationToken cancellationToken);
        Task UpdateAsync(Sale sale, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Sale>> GetSaleByMonthAndYearAsync(int month, int year, CancellationToken cancellationToken);
    }
}
