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

namespace Application.Tests.Services
{
    [TestFixture]
    public class VehicleServiceTests
    {
        private Mock<IVehicleRepository> _vehicleRepositoryMock;
        private Mock<IManufacturerRepository> _manufacturerRepositoryMock;
        private VehicleService _vehicleService;
        private CancellationToken _cancellationToken;

        [SetUp]
        public void SetUp()
        {
            _vehicleRepositoryMock = new Mock<IVehicleRepository>();
            _manufacturerRepositoryMock = new Mock<IManufacturerRepository>();
            _vehicleService = new VehicleService(_vehicleRepositoryMock.Object, _manufacturerRepositoryMock.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Test]
        public async Task GetVehicleByIdAsync_ShouldReturnSuccess_WhenVehicleExists()
        {
            var manufacturer = new Manufacturer("Ford", "Brasil", 1952, "https://ford.com");
            var vehicle = new Vehicle("Fiesta", 2020, 50000, VehicleTypeEnum.Car, "Compacto");
            vehicle.SetManufacturer(manufacturer);
            vehicle.GetType().GetProperty("Id").SetValue(vehicle, 1);

            _vehicleRepositoryMock.Setup(r => r.GetByIdAsync(1, _cancellationToken)).ReturnsAsync(vehicle);

            var result = await _vehicleService.GetVehicleByIdAsync(1, _cancellationToken);

            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Model.Should().Be("Fiesta");
        }

        [Test]
        public async Task GetVehicleByIdAsync_ShouldReturnFailure_WhenVehicleDoesNotExist()
        {
            _vehicleRepositoryMock.Setup(r => r.GetByIdAsync(1, _cancellationToken)).ReturnsAsync((Vehicle)null);

            var result = await _vehicleService.GetVehicleByIdAsync(1, _cancellationToken);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Veículo Não Localizado");
        }

        [Test]
        public async Task GetAllVehiclesAsync_ShouldReturnAllVehicles()
        {
            var manufacturer = new Manufacturer("Ford", "Brasil", 1952, "https://ford.com");
            var vehicle = new Vehicle("Fiesta", 2020, 50000, VehicleTypeEnum.Car, "Compacto");
            vehicle.SetManufacturer(manufacturer);
            var vehicles = new List<Vehicle> { vehicle };

            _vehicleRepositoryMock.Setup(r => r.GetAllAsync(_cancellationToken)).ReturnsAsync(vehicles);

            var result = await _vehicleService.GetAllVehiclesAsync(_cancellationToken);

            result.Success.Should().BeTrue();
            result.Data.Should().HaveCount(1);
            result.Data.First().Model.Should().Be("Fiesta");
        }

        [Test]
        public async Task AddVehicleAsync_ShouldReturnSuccess_WhenManufacturerExists()
        {
            var manufacturer = new Manufacturer("Ford", "Brasil", 1952, "https://ford.com");
            var dto = new VehicleDto
            {
                Model = "Fiesta",
                ManuFacturingYear = 2020,
                Price = 50000,
                Type = "Car",
                Description = "Compacto",
                ManufacturerName = "Ford"
            };

            _manufacturerRepositoryMock.Setup(r => r.GetByNameAsync("Ford", _cancellationToken)).ReturnsAsync(manufacturer);
            _vehicleRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Vehicle>(), _cancellationToken)).Returns(Task.CompletedTask);

            var result = await _vehicleService.AddVehicleAsync(dto, _cancellationToken);

            result.Success.Should().BeTrue();
        }

        [Test]
        public async Task AddVehicleAsync_ShouldReturnFailure_WhenManufacturerDoesNotExist()
        {
            var dto = new VehicleDto
            {
                Model = "Fiesta",
                ManuFacturingYear = 2020,
                Price = 50000,
                Type = "Car",
                Description = "Compacto",
                ManufacturerName = "Ford"
            };

            _manufacturerRepositoryMock.Setup(r => r.GetByNameAsync("Ford", _cancellationToken)).ReturnsAsync((Manufacturer)null);

            var result = await _vehicleService.AddVehicleAsync(dto, _cancellationToken);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Fabricante Não Localizado");
        }

        [Test]
        public async Task UpdateVehicleAsync_ShouldReturnSuccess_WhenVehicleAndManufacturerExist()
        {
            var manufacturer = new Manufacturer("Ford", "Brasil", 1952, "https://ford.com");
            var vehicle = new Vehicle("Fiesta", 2020, 50000, VehicleTypeEnum.Car, "Compacto");
            vehicle.SetManufacturer(manufacturer);
            vehicle.GetType().GetProperty("Id").SetValue(vehicle, 1);

            var dto = new VehicleDto
            {
                Id = 1,
                Model = "Focus",
                ManuFacturingYear = 2021,
                Price = 60000,
                Type = "Car",
                Description = "Sedan",
                ManufacturerName = "Ford"
            };

            _vehicleRepositoryMock.Setup(r => r.GetByIdAsync(1, _cancellationToken)).ReturnsAsync(vehicle);
            _manufacturerRepositoryMock.Setup(r => r.GetByNameAsync("Ford", _cancellationToken)).ReturnsAsync(manufacturer);
            _vehicleRepositoryMock.Setup(r => r.UpdateAsync(vehicle, _cancellationToken)).Returns(Task.CompletedTask);

            var result = await _vehicleService.UpdateVehicleAsync(dto, _cancellationToken);

            result.Success.Should().BeTrue();
        }

        [Test]
        public async Task UpdateVehicleAsync_ShouldReturnFailure_WhenVehicleDoesNotExist()
        {
            var dto = new VehicleDto
            {
                Id = 1,
                Model = "Focus",
                ManuFacturingYear = 2021,
                Price = 60000,
                Type = "Car",
                Description = "Sedan",
                ManufacturerName = "Ford"
            };

            _vehicleRepositoryMock.Setup(r => r.GetByIdAsync(1, _cancellationToken)).ReturnsAsync((Vehicle)null);

            var result = await _vehicleService.UpdateVehicleAsync(dto, _cancellationToken);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Veículo Não Localizado");
        }

        [Test]
        public async Task UpdateVehicleAsync_ShouldReturnFailure_WhenManufacturerDoesNotExist()
        {
            var manufacturer = new Manufacturer("Ford", "Brasil", 1952, "https://ford.com");
            var vehicle = new Vehicle("Fiesta", 2020, 50000, VehicleTypeEnum.Car, "Compacto");
            vehicle.SetManufacturer(manufacturer);
            vehicle.GetType().GetProperty("Id").SetValue(vehicle, 1);

            var dto = new VehicleDto
            {
                Id = 1,
                Model = "Focus",
                ManuFacturingYear = 2021,
                Price = 60000,
                Type = "Car",
                Description = "Sedan",
                ManufacturerName = "Ford"
            };

            _vehicleRepositoryMock.Setup(r => r.GetByIdAsync(1, _cancellationToken)).ReturnsAsync(vehicle);
            _manufacturerRepositoryMock.Setup(r => r.GetByNameAsync("Ford", _cancellationToken)).ReturnsAsync((Manufacturer)null);

            var result = await _vehicleService.UpdateVehicleAsync(dto, _cancellationToken);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Fabricante Não Localizado");
        }

        [Test]
        public async Task DeleteVehicleAsync_ShouldReturnSuccess_WhenVehicleExists()
        {
            var manufacturer = new Manufacturer("Ford", "Brasil", 1952, "https://ford.com");
            var vehicle = new Vehicle("Fiesta", 2020, 50000, VehicleTypeEnum.Car, "Compacto");
            vehicle.SetManufacturer(manufacturer);
            vehicle.GetType().GetProperty("Id").SetValue(vehicle, 1);

            _vehicleRepositoryMock.Setup(r => r.GetByIdAsync(1, _cancellationToken)).ReturnsAsync(vehicle);
            _vehicleRepositoryMock.Setup(r => r.DeleteAsync(1, _cancellationToken)).Returns(Task.CompletedTask);

            var result = await _vehicleService.DeleteVehicleAsync(1, _cancellationToken);

            result.Success.Should().BeTrue();
        }

        [Test]
        public async Task DeleteVehicleAsync_ShouldReturnFailure_WhenVehicleDoesNotExist()
        {
            _vehicleRepositoryMock.Setup(r => r.GetByIdAsync(1, _cancellationToken)).ReturnsAsync((Vehicle)null);

            var result = await _vehicleService.DeleteVehicleAsync(1, _cancellationToken);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Veículo Não Localizado");
        }
    }
}
