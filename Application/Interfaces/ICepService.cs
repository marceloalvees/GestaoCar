using Application.Dto;

namespace Application.Interfaces
{
    public interface ICepService
    {
        Task<MessageDto<CepInfo>> SearchZipCodeAsync(string zipCode, CancellationToken cancellationToken);
    }
}
