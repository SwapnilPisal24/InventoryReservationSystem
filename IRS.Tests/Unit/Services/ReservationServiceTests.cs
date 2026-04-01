using Xunit;
using IRS.Application.Interfaces;
using IRS.Infrastructure.Repositories;
using IRS.Domain.Entities;
using IRS.Application.Services;
using IRS.Domain.Enums;

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

        [Fact]
        public void Should_Not_Reserve_When_Stock_Is_Not_Available()
        {
            // Arrange
            var repo = new InMemoryReservationRepository();
            var item = new InventoryItem(Guid.NewGuid(), 0);

            var service = new ReservationService(repo, item);

            // Act
            var result = service.Reserve(item.Id);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Should_Allow_New_Reservation_After_Expiry()
        {
            // Arrange
            var repository = new InMemoryReservationRepository();
            var item = new InventoryItem(Guid.NewGuid(), 1);

            var service = new ReservationService(repository, item);

            // First reservation
            service.Reserve(item.Id);

            // Simulate expiry
            var reservations = repository.GetByItemId(item.Id);
            reservations[0].Expire();

            // Act
            var result = service.Reserve(item.Id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Should_Confirm_Reservation()
        {
            var repository = new InMemoryReservationRepository();
            var item = new InventoryItem(Guid.NewGuid(), 1);

            var service = new ReservationService(repository, item);

            service.Reserve(item.Id);

            var reservation = repository.GetByItemId(item.Id).First();

            service.Confirm(reservation.Id);

            Assert.Equal(ReservationStatus.Confirmed, reservation.Status);
        }
    }
}
