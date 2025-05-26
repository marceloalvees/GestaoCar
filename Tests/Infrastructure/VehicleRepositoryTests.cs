using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using FluentAssertions;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace Infrastructure.Repositories.Tests
{
    [TestFixture]
    public class VehicleRepositoryTests
    {
        private AppDbContext _context;
        private VehicleRepository _repository;
        private CancellationToken _cancellationToken;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "VehicleDbTest")
                .Options;
            _context = new AppDbContext(options);
            _repository = new VehicleRepository(_context);
            _cancellationToken = CancellationToken.None;

            // Limpa o banco antes de cada teste
            _context.Vehicles.RemoveRange(_context.Vehicles);
            _context.Manufacturers.RemoveRange(_context.Manufacturers);
            _context.SaveChanges();
        }
        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
        [Test]
        public async Task AddAsync_DeveAdicionarVeiculo()
        {
            var manufacturer = new Manufacturer("Ford", "Brasil", 1952, "https://ford.com");
            _context.Manufacturers.Add(manufacturer);
            _context.SaveChanges();

            var vehicle = new Vehicle("Fiesta", 2020, 50000, VehicleTypeEnum.Car, "Compacto");
            vehicle.SetManufacturer(manufacturer);

            await _repository.AddAsync(vehicle, _cancellationToken);

            var veiculos = await _repository.GetAllAsync(_cancellationToken);
            veiculos.Should().ContainSingle(v => v.Model == "Fiesta");
        }

        [Test]
        public async Task GetAllAsync_DeveRetornarTodosVeiculos()
        {
            var manufacturer = new Manufacturer("VW", "Alemanha", 1952, "https://vw.com");
            _context.Manufacturers.Add(manufacturer);
            _context.SaveChanges();

            var v1 = new Vehicle("Golf", 2018, 70000, VehicleTypeEnum.Car, null);
            v1.SetManufacturer(manufacturer);
            var v2 = new Vehicle("Polo", 2019, 60000, VehicleTypeEnum.Car, null);
            v2.SetManufacturer(manufacturer);
            _context.Vehicles.AddRange(v1, v2);
            _context.SaveChanges();

            var result = await _repository.GetAllAsync(_cancellationToken);

            result.Should().HaveCount(2);
            result.All(v => v.Manufacturer != null).Should().BeTrue();
        }

        [Test]
        public async Task GetByIdAsync_DeveRetornarVeiculoPorId()
        {
            var manufacturer = new Manufacturer("Honda", "Japão", 1952, "https://honda.com");
            _context.Manufacturers.Add(manufacturer);
            _context.SaveChanges();

            var vehicle = new Vehicle("Civic", 2021, 120000, VehicleTypeEnum.Car, null);
            vehicle.SetManufacturer(manufacturer);
            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();

            var result = await _repository.GetByIdAsync(vehicle.Id, _cancellationToken);

            result.Should().NotBeNull();
            result.Model.Should().Be("Civic");
        }

        [Test]
        public async Task UpdateAsync_DeveAtualizarVeiculo()
        {
            var manufacturer = new Manufacturer("Toyota", "Japão", 1952, "https://toyota.com");
            _context.Manufacturers.Add(manufacturer);
            _context.SaveChanges();

            var vehicle = new Vehicle("Corolla", 2017, 80000, VehicleTypeEnum.Car, null);
            vehicle.SetManufacturer(manufacturer);
            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();

            vehicle.Update("Corolla Altis", 2018, 90000, VehicleTypeEnum.Car, "Versão Altis");
            await _repository.UpdateAsync(vehicle, _cancellationToken);

            var atualizado = await _repository.GetByIdAsync(vehicle.Id, _cancellationToken);
            atualizado.Model.Should().Be("Corolla Altis");
            atualizado.Price.Should().Be(90000);
        }

        [Test]
        public async Task DeleteAsync_DeveRemoverVeiculo()
        {
            var manufacturer = new Manufacturer("Fiat", "Itália", 1952, "https://fiat.com");
            _context.Manufacturers.Add(manufacturer);
            _context.SaveChanges();

            var vehicle = new Vehicle("Uno", 2010, 20000, VehicleTypeEnum.Car, null);
            vehicle.SetManufacturer(manufacturer);
            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();

            await _repository.DeleteAsync(vehicle.Id, _cancellationToken);

            var result = await _repository.GetByIdAsync(vehicle.Id, _cancellationToken);
            result.Should().BeNull();
        }

        [Test]
        public async Task GetByModelAsync_DeveRetornarVeiculoPorModelo_IgnorandoCase()
        {
            var manufacturer = new Manufacturer("Chevrolet", "EUA", 1952, "https://chevrolet.com");
            _context.Manufacturers.Add(manufacturer);
            _context.SaveChanges();

            var vehicle = new Vehicle("Onix", 2022, 75000, VehicleTypeEnum.Car, null);
            vehicle.SetManufacturer(manufacturer);
            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();

            var result = await _repository.GetByModelAsync("onix", _cancellationToken);

            result.Should().NotBeNull();
            result.Model.Should().Be("Onix");
        }
    }
}
