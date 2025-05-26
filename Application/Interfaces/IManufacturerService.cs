using Application.Dto;

namespace Application.Interfaces
{
    public interface IManufacturerService
    {
        Task<MessageDto<ManufacturerDto>> GetManufacturerByIdAsync(int idm, CancellationToken cancellationToken);
        Task<MessageDto<IEnumerable<ManufacturerDto>>> GetAllManufacturersAsync(CancellationToken cancellationToken);
        Task<MessageDto<object>> AddManufacturerAsync(ManufacturerDto manufacturerDto, CancellationToken cancellationToken);
        Task<MessageDto<object>> UpdateManufacturerAsync(ManufacturerDto manufacturerDto, CancellationToken cancellationToken);
        Task<MessageDto<object>> DeleteManufacturerAsync(int id, CancellationToken cancellationToken);
        Task<MessageDto<object>> ManufacturerExistsByNameAsync(string name, CancellationToken cancellationToken);
    }
}
