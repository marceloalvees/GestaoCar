using Application.Dto;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class DealershipService : IDealershipService
    {
        private readonly IDealershipRepository _dealershipRepository;
        public DealershipService(IDealershipRepository dealershipRepository)
        {
            _dealershipRepository = dealershipRepository;
        }
        public async Task<MessageDto<DealershipDto>> GetDealershipByIdAsync(int id, CancellationToken cancellationToken)
        {
            var dealership = await _dealershipRepository.GetByIdAsync(id, cancellationToken);
            if (dealership == null)
                return MessageDto<DealershipDto>.FailureResult("Concessionária Não Localizada");
            var dto = MapToDto(dealership);
            return MessageDto<DealershipDto>.SuccessResult(dto);
        }
        public async Task<MessageDto<IEnumerable<DealershipDto>>> GetAllDealershipsAsync(CancellationToken cancellationToken)
        {
            var dealerships = await _dealershipRepository.GetAllAsync(cancellationToken);
            IEnumerable<DealershipDto> dealershipDtos = dealerships.Select(MapToDto);
            return MessageDto<IEnumerable<DealershipDto>>.SuccessResult(dealershipDtos);
        }
        public async Task<MessageDto<object>> AddDealershipAsync(DealershipDto dealershipDto, CancellationToken cancellationToken)
        {
            var dealership = new Dealership(
                dealershipDto.Name,
                dealershipDto.Address,
                dealershipDto.City,
                dealershipDto.State,
                dealershipDto.ZipCode,
                dealershipDto.Phone,
                dealershipDto.Email,
                dealershipDto.MaxVehicleCapacity
            );
            await _dealershipRepository.AddAsync(dealership, cancellationToken);
            return MessageDto<object>.SuccessResult(null);
        }
        public async Task<MessageDto<object>> UpdateDealershipAsync(DealershipDto dealershipDto, CancellationToken cancellationToken)
        {
            var dealership = await _dealershipRepository.GetByIdAsync(dealershipDto.Id.GetValueOrDefault(), cancellationToken);
            if (dealership == null)
                return MessageDto<object>.FailureResult("Concessionária Não Localizada");
            dealership.Update(
                dealershipDto.Name,
                dealershipDto.Address,
                dealershipDto.City,
                dealershipDto.State,
                dealershipDto.ZipCode,
                dealershipDto.Phone,
                dealershipDto.Email,
                dealershipDto.MaxVehicleCapacity
            );
            await _dealershipRepository.UpdateAsync(dealership, cancellationToken);
            return MessageDto<object>.SuccessResult(null);
        }
        public async Task<MessageDto<object>> DeleteDealershipAsync(int id, CancellationToken cancellationToken)
        {
            var dealership = await _dealershipRepository.GetByIdAsync(id, cancellationToken);
            if (dealership == null)
                return MessageDto<object>.FailureResult("Concessionária Não Localizada");
            await _dealershipRepository.DeleteAsync(id, cancellationToken);
            return MessageDto<object>.SuccessResult(null);
        }
        
        private DealershipDto MapToDto(Dealership dealership)
        {
            return new DealershipDto
            {
                Name = dealership.Name,
                Address = dealership.Address,
                Phone = dealership.Phone,
                Email = dealership.Email,
                City = dealership.City,
                State = dealership.State,
                ZipCode = dealership.ZipCode,
                MaxVehicleCapacity = dealership.MaxVehicleCapacity,
            };
        }
    }
}
