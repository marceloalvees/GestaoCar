using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using FluentAssertions;
using Domain.Enums;

namespace Infrastructure.Repositories.Tests
{
    [TestFixture]
    public class SaleRepositoryTests
    {
        private AppDbContext _context;
        private SaleRepository _repository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);
            _repository = new SaleRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private Sale CreateSale()
        {
            var vehicle = new Vehicle("Model X", 2022, 100000, VehicleTypeEnum.Car, "Desc");
            var dealership = new Dealership("Dealership", "Address", "City", "State", "12345", "123456789", "email@test.com", 10);
            var client = new Client("Client", "12345678909", "999999999");
            _context.Vehicles.Add(vehicle);
            _context.Dealerships.Add(dealership);
            _context.Clients.Add(client);
            _context.SaveChanges();

            return new Sale(vehicle.Id, dealership.Id, client.Id, DateTime.Now, 90000);
        }

        [Test]
        public async Task AddAsync_ShouldAddSale()
        {
            var sale = CreateSale();
            await _repository.AddAsync(sale, CancellationToken.None);

            var result = await _repository.GetByIdAsync(sale.Id, CancellationToken.None);
            result.Should().NotBeNull();
            result.SalePrice.Should().Be(90000);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnSaleWithIncludes()
        {
            var sale = CreateSale();
            await _repository.AddAsync(sale, CancellationToken.None);

            var result = await _repository.GetByIdAsync(sale.Id, CancellationToken.None);
            result.Should().NotBeNull();
            result.Vehicle.Should().NotBeNull();
            result.Dealership.Should().NotBeNull();
            result.Client.Should().NotBeNull();
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllSales()
        {
            var sale1 = CreateSale();
            var sale2 = CreateSale();
            await _repository.AddAsync(sale1, CancellationToken.None);
            await _repository.AddAsync(sale2, CancellationToken.None);

            var result = await _repository.GetAllAsync(CancellationToken.None);
            result.Should().HaveCount(2);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateSale()
        {
            var sale = CreateSale();
            await _repository.AddAsync(sale, CancellationToken.None);

            sale.UpdateSale(DateTime.Now.AddDays(1), 95000);
            await _repository.UpdateAsync(sale, CancellationToken.None);

            var updated = await _repository.GetByIdAsync(sale.Id, CancellationToken.None);
            updated.SalePrice.Should().Be(95000);
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveSale()
        {
            var sale = CreateSale();
            await _repository.AddAsync(sale, CancellationToken.None);

            await _repository.DeleteAsync(sale.Id, CancellationToken.None);
            var result = await _repository.GetByIdAsync(sale.Id, CancellationToken.None);
            result.Should().BeNull();
        }

        [Test]
        public async Task GetSaleByMonthAndYearAsync_ShouldReturnSalesForGivenMonthAndYear()
        {
            var sale = CreateSale();
            sale.UpdateSale(new DateTime(2023, 5, 10), 90000);
            await _repository.AddAsync(sale, CancellationToken.None);

            var result = await _repository.GetSaleByMonthAndYearAsync(5, 2023, CancellationToken.None);
            result.Should().ContainSingle();
        }
    }
}
