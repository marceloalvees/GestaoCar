using Application.Dto;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Application.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IManufacturerRepository _manufacturerRepository;
        public VehicleService(IVehicleRepository vehicleRepository, IManufacturerRepository manufacturerRepository)
        {
            _vehicleRepository = vehicleRepository;
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task<MessageDto<VehicleDto>> GetVehicleByIdAsync(int id, CancellationToken cancellationToken)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id, cancellationToken);
            if (vehicle == null)
                return MessageDto<VehicleDto>.FailureResult("Veículo Não Localizado");
            var dto = MapToDto(vehicle);
            return MessageDto<VehicleDto>.SuccessResult(dto);
        }
        public async Task<MessageDto<IEnumerable<VehicleDto>>> GetAllVehiclesAsync(CancellationToken cancellationToken)
        {
            var vehicles = await _vehicleRepository.GetAllAsync(cancellationToken);
            IEnumerable<VehicleDto> vehicleDtos = vehicles.Select(MapToDto);
            return MessageDto<IEnumerable<VehicleDto>>.SuccessResult(vehicleDtos);
        }
        public async Task<MessageDto<object>> AddVehicleAsync(VehicleDto vehicleDto, CancellationToken cancellationToken)
        {
            var manufacturer = await _manufacturerRepository.GetByNameAsync(vehicleDto.ManufacturerName, cancellationToken);
            if (manufacturer == null)
                return MessageDto<object>.FailureResult("Fabricante Não Localizado");

            var type = (VehicleTypeEnum)Enum.Parse(typeof(VehicleTypeEnum), vehicleDto.Type, true);
            var vehicle = new Vehicle(
                vehicleDto.Model,
                vehicleDto.ManuFacturingYear,
                vehicleDto.Price,
                type,
                vehicleDto.Description
            );
            vehicle.SetManufacturer(manufacturer);
            await _vehicleRepository.AddAsync(vehicle, cancellationToken);
            return MessageDto<object>.SuccessResult(null);
        }
        public async Task<MessageDto<object>> UpdateVehicleAsync(VehicleDto vehicleDto, CancellationToken cancellationToken)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleDto.Id.GetValueOrDefault(), cancellationToken);
            if (vehicle == null)
                return MessageDto<object>.FailureResult("Veículo Não Localizado");

            var manufacturer = await _manufacturerRepository.GetByNameAsync(vehicleDto.ManufacturerName, cancellationToken);
            if (manufacturer == null)
                return MessageDto<object>.FailureResult("Fabricante Não Localizado");

            vehicle.Update(
                vehicleDto.Model,
                vehicleDto.ManuFacturingYear,
                vehicleDto.Price,
                (VehicleTypeEnum)Enum.Parse(typeof(VehicleTypeEnum), vehicleDto.Type, true),
                vehicleDto.Description
            );
            await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);
            return MessageDto<object>.SuccessResult(null);
        }

        public async Task<MessageDto<object>> DeleteVehicleAsync(int id, CancellationToken cancellationToken)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id, cancellationToken);
            if (vehicle == null)
                return MessageDto<object>.FailureResult("Veículo Não Localizado");
            await _vehicleRepository.DeleteAsync(id, cancellationToken);
            return MessageDto<object>.SuccessResult(null);
        }
        private VehicleDto MapToDto(Vehicle vehicle)
        {
            return new VehicleDto
            {
                Id = vehicle.Id,
                Model = vehicle.Model,
                ManuFacturingYear = vehicle.ManuFacturingYear,
                Price = vehicle.Price,
                Type = vehicle.VehicleType.ToString(),
                Description = vehicle.Description,
                ManufacturerName = vehicle.Manufacturer.Name
            };
        }

    }
}
