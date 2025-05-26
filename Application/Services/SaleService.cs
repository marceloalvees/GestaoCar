using Application.Dto;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IDealershipRepository _dealershipRepository;
        private readonly IClientRepository _clientRepository;
        public SaleService(ISaleRepository saleRepository, IVehicleRepository vehicleRepository, IDealershipRepository dealershipRepository, IClientRepository clientRepository)
        {
            _saleRepository = saleRepository;
            _vehicleRepository = vehicleRepository;
            _dealershipRepository = dealershipRepository;
            _clientRepository = clientRepository;
        }
        public async Task<MessageDto<SaleDto>> GetSaleByIdAsync(int id, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetByIdAsync(id, cancellationToken);
            if (sale == null)
                return MessageDto<SaleDto>.FailureResult("Venda Não Localizada");
            var dto = MapToDto(sale);
            return MessageDto<SaleDto>.SuccessResult(dto);
        }
        public async Task<MessageDto<IEnumerable<SaleDto>>> GetAllSalesAsync(CancellationToken cancellationToken)
        {
            var sales = await _saleRepository.GetAllAsync(cancellationToken);
            IEnumerable<SaleDto> saleDtos = sales.Select(MapToDto);
            return MessageDto<IEnumerable<SaleDto>>.SuccessResult(saleDtos);
        }
        public async Task<MessageDto<object>> AddSaleAsync(SaleDto saleDto, CancellationToken cancellationToken)
        {
            var vehicle = await _vehicleRepository.GetByModelAsync(saleDto.VehicleName, cancellationToken);
            if (vehicle == null)
                return MessageDto<object>.FailureResult("Veículo Não Localizado");
            var dealership = await _dealershipRepository.GetByNameAsync(saleDto.DealershipName, cancellationToken);
            if (dealership == null)
                return MessageDto<object>.FailureResult("Concessionária Não Localizada");
            var idClient = await CreateClientAsync(saleDto.CustomerName, saleDto.CustomerCpf, saleDto.CustomerPhone, cancellationToken);

            var sale = new Sale(vehicle.Id, dealership.Id, idClient, saleDto.SaleDate, saleDto.SalePrice);
            await _saleRepository.AddAsync(sale, cancellationToken);
            return MessageDto<object>.SuccessResult(null);
        }
        public async Task<MessageDto<object>> UpdateSaleAsync(SaleDto saleDto, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetByIdAsync(saleDto.Id.GetValueOrDefault(), cancellationToken);
            sale.UpdateSale(saleDto.SaleDate, saleDto.SalePrice);
            await _saleRepository.UpdateAsync(sale, cancellationToken);
            return MessageDto<object>.SuccessResult(null);
        }
        public async Task<MessageDto<object>> DeleteSaleAsync(int id, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetByIdAsync(id, cancellationToken);
            if (sale == null)
                return MessageDto<object>.FailureResult("Venda Não Localizada");
            await _saleRepository.DeleteAsync(id, cancellationToken);
            return MessageDto<object>.SuccessResult(null);
        }
        public async Task<MessageDto<bool>> ClientExistAsync(string Cpf, CancellationToken cancellation)
        {
            var client = await _clientRepository.GetClientByCpfAsync(Cpf, cancellation);
            if (client == null)
                return MessageDto<bool>.SuccessResult(true);
            return MessageDto<bool>.SuccessResult(false);
        }

        public async Task<MessageDto<IEnumerable<SaleDto>>> GetSaleByMonthAndYearAsync(int month, int year, CancellationToken cancellationToken)
        {
            var sales = await _saleRepository.GetSaleByMonthAndYearAsync(month, year, cancellationToken);
            IEnumerable<SaleDto> saleDtos = sales.Select(MapToDto);
            return MessageDto<IEnumerable<SaleDto>>.SuccessResult(saleDtos);
        }

        private SaleDto MapToDto(Sale sale)
        {
            return new SaleDto
            {
                Id = sale.Id,
                VehicleName = sale.Vehicle.Model,
                DealershipName = sale.Dealership.Name,
                SaleDate = sale.SaleDate,
                SalePrice = sale.SalePrice,
                CustomerName = sale.Client.Name,
                Type = GetType(sale.Vehicle.VehicleType.ToString())

            };
        }

        private async Task<int> CreateClientAsync(string name, string cpf, string phone, CancellationToken cancellationToken)
        {
            var client = new Client(name, cpf, phone);
            await _clientRepository.AddAsync(client, cancellationToken);
            return client.Id;
        }
        private static string GetType(string type)
        {
            return type.ToLowerInvariant() switch
            {
                "car" => "Carro",
                "truck" => "Caminhão",
                _ => "Moto"
            };
        }

    }

}
