using Application.Dto;

namespace Application.Interfaces
{
    public interface IDealershipService
    {
        Task<MessageDto<DealershipDto>> GetDealershipByIdAsync(int id, CancellationToken cancellationToken);
        Task<MessageDto<IEnumerable<DealershipDto>>> GetAllDealershipsAsync(CancellationToken cancellationToken);
        Task<MessageDto<object>> AddDealershipAsync(DealershipDto dealershipDto, CancellationToken cancellationToken);
        Task<MessageDto<object>> UpdateDealershipAsync(DealershipDto dealershipDto, CancellationToken cancellationToken);
        Task<MessageDto<object>> DeleteDealershipAsync(int id, CancellationToken cancellationToken);

    }
}
