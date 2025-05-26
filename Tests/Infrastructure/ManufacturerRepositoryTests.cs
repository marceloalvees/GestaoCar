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
    public class ManufacturerRepositoryTests
    {
        private AppDbContext _context;
        private ManufacturerRepository _repository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new AppDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            _repository = new ManufacturerRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task AddAsync_ShouldAddManufacturer()
        {
            var manufacturer = new Manufacturer("Ford", "USA", 1952, "https://ford.com");
            await _repository.AddAsync(manufacturer, CancellationToken.None);

            var result = await _context.Manufacturers.FirstOrDefaultAsync(m => m.Name == "Ford");
            result.Should().NotBeNull();
            result.Name.Should().Be("Ford");
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnManufacturerWithVehicles()
        {
            var manufacturer = new Manufacturer("Toyota", "Japan", 1952, "https://toyota.com");
            var vehicle = new Vehicle("Corolla", 2020, 80000, VehicleTypeEnum.Car, null);
            manufacturer.Vehicles = new List<Vehicle> { vehicle };
            _context.Manufacturers.Add(manufacturer);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(manufacturer.Id, CancellationToken.None);

            result.Should().NotBeNull();
            result.Vehicles.Should().HaveCount(1);
            result.Name.Should().Be("Toyota");
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllManufacturers()
        {
            _context.Manufacturers.Add(new Manufacturer("Honda", "Japan", 1952, "https://honda.com"));
            _context.Manufacturers.Add(new Manufacturer("BMW", "Germany", 1954, "https://bmw.com"));
            await _context.SaveChangesAsync();

            var result = await _repository.GetAllAsync(CancellationToken.None);

            result.Should().HaveCount(2);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateManufacturer()
        {
            var manufacturer = new Manufacturer("Fiat", "Italy", 1952, "https://fiat.com");
            _context.Manufacturers.Add(manufacturer);
            await _context.SaveChangesAsync();

            manufacturer.Name = "FIAT Updated";
            await _repository.UpdateAsync(manufacturer, CancellationToken.None);

            var updated = await _context.Manufacturers.FindAsync(manufacturer.Id);
            updated.Name.Should().Be("FIAT Updated");
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveManufacturer()
        {
            var manufacturer = new Manufacturer("Renault", "France", 1952, "https://renault.com");
            _context.Manufacturers.Add(manufacturer);
            await _context.SaveChangesAsync();

            await _repository.DeleteAsync(manufacturer.Id, CancellationToken.None);

            var deleted = await _context.Manufacturers.FindAsync(manufacturer.Id);
            deleted.Should().BeNull();
        }

        [Test]
        public async Task GetByNameAsync_ShouldReturnManufacturer()
        {
            var manufacturer = new Manufacturer("Chevrolet", "USA", 1952, "https://chevrolet.com");
            _context.Manufacturers.Add(manufacturer);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByNameAsync("Chevrolet", CancellationToken.None);

            result.Should().NotBeNull();
            result.Name.Should().Be("Chevrolet");
        }

        [Test]
        public async Task ExistsAsync_ShouldReturnTrueIfExists()
        {
            var manufacturer = new Manufacturer("Hyundai", "South Korea", 1967, "https://hyundai.com");
            _context.Manufacturers.Add(manufacturer);
            await _context.SaveChangesAsync();

            var exists = await _repository.ExistsAsync("Hyundai", CancellationToken.None);

            exists.Should().BeTrue();
        }

        [Test]
        public async Task ExistsAsync_ShouldReturnFalseIfNotExists()
        {
            var exists = await _repository.ExistsAsync("NonExistent", CancellationToken.None);
            exists.Should().BeFalse();
        }
    }
}
