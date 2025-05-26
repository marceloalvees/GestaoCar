using Application.Dto;

namespace Application.Interfaces
{
    public interface IExcelService
    {
        Task<byte[]> GenerateMonthlyReportInExcelAsync(IEnumerable<SaleDto> vendas);
        Task<byte[]> GenerateMonthlyReportInPdfAsync(IEnumerable<SaleDto> vendas);
    }
}
