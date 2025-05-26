using Application.Dto;

namespace Application.Interfaces
{
    public interface ISaleService
    {
        public Task<MessageDto<SaleDto>> GetSaleByIdAsync(int id, CancellationToken cancellationToken);
        public Task<MessageDto<IEnumerable<SaleDto>>> GetAllSalesAsync(CancellationToken cancellationToken);
        public Task<MessageDto<object>> AddSaleAsync(SaleDto saleDto, CancellationToken cancellationToken);
        public Task<MessageDto<object>> UpdateSaleAsync(SaleDto saleDto, CancellationToken cancellationToken);
        public Task<MessageDto<object>> DeleteSaleAsync(int id, CancellationToken cancellationToken);
        Task<MessageDto<bool>> ClientExistAsync(string Cpf, CancellationToken cancellation);
        Task<MessageDto<IEnumerable<SaleDto>>> GetSaleByMonthAndYearAsync(int month, int year, CancellationToken cancellationToken);
    }
}
