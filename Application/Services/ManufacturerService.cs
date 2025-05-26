using Application.Dto;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        public ManufacturerService(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task<MessageDto<ManufacturerDto>> GetManufacturerByIdAsync(int id, CancellationToken cancellationToken)
        {
            var manufacturer = await _manufacturerRepository.GetByIdAsync(id, cancellationToken);
            if (manufacturer == null)
                return MessageDto<ManufacturerDto>.FailureResult("Fabricante Não Localizado");
            var dto = MapToDto(manufacturer);
            return MessageDto<ManufacturerDto>.SuccessResult(dto);
        }

        public async Task<MessageDto<IEnumerable<ManufacturerDto>>> GetAllManufacturersAsync(CancellationToken cancellationToken)
        {
            var manufacturers = await _manufacturerRepository.GetAllAsync(cancellationToken);

            IEnumerable<ManufacturerDto> manufacturerDtos = manufacturers.Select(MapToDto);
            return MessageDto<IEnumerable<ManufacturerDto>>.SuccessResult(manufacturerDtos);
        }

        public async Task<MessageDto<object>> AddManufacturerAsync(ManufacturerDto manufacturerDto, CancellationToken cancellationToken)
        {
            var existsResult = await _manufacturerRepository.ExistsAsync(manufacturerDto.Name, cancellationToken);
            if (existsResult)
            {
                return MessageDto<object>.FailureResult("Fabricante já cadastrado com esse nome.");

            }
            var manufacturer = new Domain.Entities.Manufacturer(
                manufacturerDto.Name,
                manufacturerDto.Country,
                manufacturerDto.FoundationYear,
                manufacturerDto.Website
            );
            await _manufacturerRepository.AddAsync(manufacturer, cancellationToken);
            return MessageDto<object>.SuccessResult(null);

        }

        public async Task<MessageDto<object>> UpdateManufacturerAsync(ManufacturerDto manufacturerDto, CancellationToken cancellationToken)
        {
            var manufacturer = await _manufacturerRepository.GetByIdAsync(manufacturerDto.Id.GetValueOrDefault(), cancellationToken);
            if (manufacturer == null)
                return MessageDto<object>.FailureResult("Fabricante Não Localizado");

            manufacturer.Country = manufacturerDto.Country;
            manufacturer.Website = manufacturerDto.Website;
            await _manufacturerRepository.UpdateAsync(manufacturer, cancellationToken);

            return MessageDto<object>.SuccessResult(null);
        }

        public async Task<MessageDto<object>> DeleteManufacturerAsync(int id, CancellationToken cancellationToken)
        {
            var manufacturer = await _manufacturerRepository.GetByIdAsync(id, cancellationToken);
            if (manufacturer == null)
                return MessageDto<object>.FailureResult("Fabricante Não Localizado"); 

            await _manufacturerRepository.DeleteAsync(id, cancellationToken);
            return MessageDto<object>.SuccessResult(null);
        }

        public async Task<MessageDto<object>> ManufacturerExistsByNameAsync(string name, CancellationToken cancellationToken)
        {
            var exists = await _manufacturerRepository.ExistsAsync(name, cancellationToken);
            if (exists)
                return MessageDto<object>.SuccessResult(null);
            else
                return MessageDto<object>.FailureResult("Fabricante Não Localizado");
        }

        private ManufacturerDto MapToDto(Manufacturer m) =>
            new ManufacturerDto
            {
                Id = m.Id,
                Name = m.Name,
                Country = m.Country,
                FoundationYear = m.FoundationYear,
                Website = m.Website
            };
    }

}
