using IRS.Tests.Fakes;
using Xunit;
using IRS.Application.Interfaces;
using IRS.Infrastructure.Repositories;
using IRS.Domain.Entities;
using IRS.Application.Services;

namespace IRS.Tests.Unit.Services
{
    public class ReservationServiceTests
    {
        [Fact]
        public void Should_Reserve_When_Stock_Is_Available()
        {
            // Arrange
            var repo = new InMemoryReservationRepository();
            var item = new InventoryItem(Guid.NewGuid(), 1);

            var service = new ReservationService(repo, item);

            // Act
            var result = service.Reserve(item.Id);

            // Assert
            Assert.True(result);
        }
    }
}
