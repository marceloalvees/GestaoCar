using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Dto;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Application.Services.Tests
{
    [TestFixture]
    public class SaleServiceTests
    {
        private Mock<ISaleRepository> _saleRepositoryMock;
        private Mock<IVehicleRepository> _vehicleRepositoryMock;
        private Mock<IDealershipRepository> _dealershipRepositoryMock;
        private Mock<IClientRepository> _clientRepositoryMock;
        private SaleService _service;
        private CancellationToken _cancellationToken;

        [SetUp]
        public void SetUp()
        {
            _saleRepositoryMock = new Mock<ISaleRepository>();
            _vehicleRepositoryMock = new Mock<IVehicleRepository>();
            _dealershipRepositoryMock = new Mock<IDealershipRepository>();
            _clientRepositoryMock = new Mock<IClientRepository>();
            _service = new SaleService(
                _saleRepositoryMock.Object,
                _vehicleRepositoryMock.Object,
                _dealershipRepositoryMock.Object,
                _clientRepositoryMock.Object
            );
            _cancellationToken = CancellationToken.None;
        }

        [Test]
        public async Task GetSaleByIdAsync_SaleExists_ReturnsSuccess()
        {
            var sale = CreateSale();
            _saleRepositoryMock.Setup(r => r.GetByIdAsync(1, _cancellationToken)).ReturnsAsync(sale);

            var result = await _service.GetSaleByIdAsync(1, _cancellationToken);

            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(sale.Id);
        }

        [Test]
        public async Task GetSaleByIdAsync_SaleNotFound_ReturnsFailure()
        {
            _saleRepositoryMock.Setup(r => r.GetByIdAsync(1, _cancellationToken)).ReturnsAsync((Sale)null);

            var result = await _service.GetSaleByIdAsync(1, _cancellationToken);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Venda Não Localizada");
        }

        [Test]
        public async Task AddSaleAsync_VehicleNotFound_ReturnsFailure()
        {
            _vehicleRepositoryMock.Setup(r => r.GetByModelAsync(It.IsAny<string>(), _cancellationToken)).ReturnsAsync((Vehicle)null);
            var dto = new SaleDto { VehicleName = "X", DealershipName = "Y", CustomerName = "Z", CustomerCpf = "123", CustomerPhone = "999", SaleDate = DateTime.Now, SalePrice = 100 };

            var result = await _service.AddSaleAsync(dto, _cancellationToken);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Veículo Não Localizado");
        }

        [Test]
        public async Task AddSaleAsync_DealershipNotFound_ReturnsFailure()
        {
            _vehicleRepositoryMock.Setup(r => r.GetByModelAsync(It.IsAny<string>(), _cancellationToken)).ReturnsAsync(new Vehicle("X", 2020, 100, VehicleTypeEnum.Car, null));
            _dealershipRepositoryMock.Setup(r => r.GetByNameAsync(It.IsAny<string>(), _cancellationToken)).ReturnsAsync((Dealership)null);
            var dto = new SaleDto { VehicleName = "X", DealershipName = "Y", CustomerName = "Z", CustomerCpf = "123", CustomerPhone = "999", SaleDate = DateTime.Now, SalePrice = 100 };

            var result = await _service.AddSaleAsync(dto, _cancellationToken);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Concessionária Não Localizada");
        }

        [Test]
        public async Task AddSaleAsync_ValidData_AddsSale()
        {
            var vehicle = new Vehicle("X", 2020, 100, VehicleTypeEnum.Car, null) { Id = 1 };
            var dealership = new Dealership("Yaaaaa", "Abcadsas", "Bcd", "Cef", "Desadasds", "Enn", "Feas@gmail.com", 520) { Id = 2 };
            _vehicleRepositoryMock.Setup(r => r.GetByModelAsync(It.IsAny<string>(), _cancellationToken)).ReturnsAsync(vehicle);
            _dealershipRepositoryMock.Setup(r => r.GetByNameAsync(It.IsAny<string>(), _cancellationToken)).ReturnsAsync(dealership);
            _clientRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Client>(), _cancellationToken)).Callback<Client, CancellationToken>((c, t) => c.Id = 3).Returns(Task.CompletedTask);
            var dto = new SaleDto { VehicleName = "X", DealershipName = "Y", CustomerName = "Zadasdf", CustomerCpf = "39053344705", CustomerPhone = "9988888889", SaleDate = DateTime.Now, SalePrice = 100 };

            var result = await _service.AddSaleAsync(dto, _cancellationToken);

            result.Success.Should().BeTrue();
            _saleRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Sale>(), _cancellationToken), Times.Once);
        }

        [Test]
        public async Task DeleteSaleAsync_SaleNotFound_ReturnsFailure()
        {
            _saleRepositoryMock.Setup(r => r.GetByIdAsync(1, _cancellationToken)).ReturnsAsync((Sale)null);

            var result = await _service.DeleteSaleAsync(1, _cancellationToken);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Venda Não Localizada");
        }

        [Test]
        public async Task DeleteSaleAsync_SaleFound_DeletesSale()
        {
            var sale = CreateSale();
            _saleRepositoryMock.Setup(r => r.GetByIdAsync(1, _cancellationToken)).ReturnsAsync(sale);

            var result = await _service.DeleteSaleAsync(1, _cancellationToken);

            result.Success.Should().BeTrue();
            _saleRepositoryMock.Verify(r => r.DeleteAsync(1, _cancellationToken), Times.Once);
        }

        [Test]
        public async Task ClientExistAsync_ClientNotFound_ReturnsTrue()
        {
            _clientRepositoryMock.Setup(r => r.GetClientByCpfAsync("123", _cancellationToken)).ReturnsAsync((Client)null);

            var result = await _service.ClientExistAsync("123", _cancellationToken);

            result.Success.Should().BeTrue();
            result.Data.Should().BeTrue();
        }

        [Test]
        public async Task ClientExistAsync_ClientFound_ReturnsFalse()
        {
            _clientRepositoryMock.Setup(r => r.GetClientByCpfAsync("123", _cancellationToken)).ReturnsAsync(new Client("Aasda", "11144477735", "999"));

            var result = await _service.ClientExistAsync("123", _cancellationToken);

            result.Success.Should().BeTrue();
            result.Data.Should().BeFalse();
        }

        private Sale CreateSale()
        {
            var vehicle = new Vehicle("Model", 2020, 100, VehicleTypeEnum.Car, null) { Id = 1 };
            var dealership = new Dealership("Dealership", "Address", "City", "State", "Zipcodeee", "Phone", "Email@gmail.com", 10) { Id = 2 };
            var client = new Client("Client", "11144477735", "999") { Id = 3 };
            var sale = new Sale(vehicle.Id, dealership.Id, client.Id, DateTime.Now, 100) { Id = 4 };
            sale.Vehicle = vehicle;
            sale.Dealership = dealership;
            sale.Client = client;
            return sale;
        }
    }
}
