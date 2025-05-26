using Application.Dto;

namespace Application.Interfaces
{
    public interface IVehicleService
    {
        Task<MessageDto<VehicleDto>> GetVehicleByIdAsync(int id, CancellationToken cancellationToken);
        Task<MessageDto<IEnumerable<VehicleDto>>> GetAllVehiclesAsync(CancellationToken cancellationToken);
        Task<MessageDto<object>> AddVehicleAsync(VehicleDto vehicleDto, CancellationToken cancellationToken);
        Task<MessageDto<object>> UpdateVehicleAsync(VehicleDto vehicleDto, CancellationToken cancellationToken);
        Task<MessageDto<object>> DeleteVehicleAsync(int id, CancellationToken cancellationToken);
    }
}
