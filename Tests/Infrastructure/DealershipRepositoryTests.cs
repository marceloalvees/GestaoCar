using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Tests.Infrastructure.Repositories
{
    public class DealershipRepositoryTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "DealershipTestDb_" + System.Guid.NewGuid())
                .Options;
            return new AppDbContext(options);
        }

        private Dealership CreateDealership(int id = 1, string name = "Test Dealer")
        {
            return new Dealership(name, "Address", "City", "State", "00000-000", "123456789", "test@email.com", 10)
            {
                Id = id
            };
        }

        [Test]
        public async Task AddAsync_ShouldAddDealership()
        {
            using var context = GetDbContext();
            var repo = new DealershipRepository(context);
            var dealership = CreateDealership();

            await repo.AddAsync(dealership, CancellationToken.None);

            context.Should().NotBeNull();
            context.Dealerships.Should().HaveCount(1);
            context.Dealerships.First().Name.Should().Be("Test Dealer");
        }


        [Test]
        public async Task GetByIdAsync_ShouldReturnCorrectDealership()
        {
            // Arrange
            using var context = GetDbContext();
            var dealership = CreateDealership(1, "Dealer1");
            context.Dealerships.Add(dealership);
            context.SaveChanges();
            var repo = new DealershipRepository(context);

            // Act
            var result = await repo.GetByIdAsync(1, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Dealer1");
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateDealership()
        {
            // Arrange
            using var context = GetDbContext();
            var dealership = CreateDealership(1, "OldName");
            context.Dealerships.Add(dealership);
            context.SaveChanges();
            var repo = new DealershipRepository(context);

            // Act
            dealership.Update("NewName", dealership.Address, dealership.City, dealership.State, dealership.ZipCode, dealership.Phone, dealership.Email, dealership.MaxVehicleCapacity);
            await repo.UpdateAsync(dealership, CancellationToken.None);

            // Assert
            var updated = context.Dealerships.First();
            updated.Should().NotBeNull();
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveDealership()
        {
            // Arrange
            using var context = GetDbContext();
            var dealership = CreateDealership(1, "ToDelete");
            context.Dealerships.Add(dealership);
            context.SaveChanges();
            var repo = new DealershipRepository(context);
            // Act
            await repo.DeleteAsync(1, CancellationToken.None);

            // Assert
            context.Dealerships.Should().BeEmpty();
        }

        [Test]
        public async Task GetByNameAsync_ShouldReturnCorrectDealership_IgnoringCase()
        {
            // Arrange
            using var context = GetDbContext();
            context.Dealerships.Add(CreateDealership(1, "CaseTest"));
            context.SaveChanges();
            var repo = new DealershipRepository(context);

            // Act
            var result = await repo.GetByNameAsync("casetest", CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("CaseTest");
        }
    }
}
