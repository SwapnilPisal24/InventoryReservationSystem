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

        private (ReservationService service, InMemoryReservationRepository repository, InventoryItem item)
        CreateTestSetup(int stock = 1)
        {
            var repository = new InMemoryReservationRepository();
            var item = new InventoryItem(Guid.NewGuid(), stock);
            var service = new ReservationService(repository, item);

            return (service, repository, item);
        }


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
            Assert.Equal(ReservationResult.Success, result);
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
            Assert.Equal(ReservationResult.OutOfStock, result);
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
            Assert.Equal(ReservationResult.Success, result);
        }

        [Fact]
        public void Should_Confirm_Reservation()
        {
            var (service, repository, item) = CreateTestSetup();

            service.Reserve(item.Id);

            var reservation = repository.GetByItemId(item.Id).First();

            service.Confirm(reservation.Id);

            Assert.Equal(ReservationStatus.Confirmed, reservation.Status);
        }

        [Fact]
        public void Should_Allow_Only_One_Reservation_When_Concurrent_Requests()
        {
            // Arrange
            var repository = new InMemoryReservationRepository();
            var item = new InventoryItem(Guid.NewGuid(), 1);

            var service = new ReservationService(repository, item);

            int successCount = 0;

            // Act
            Parallel.For(0, 500, i =>
            {
                var result = service.Reserve(item.Id);

                switch (result)
                {
                    case ReservationResult.Success:
                        Interlocked.Increment(ref successCount);// to increment success count in thread safe way
                        break;

                    case ReservationResult.AlreadyConfirmed:
                    case ReservationResult.Expired:
                    case ReservationResult.OutOfStock:
                        break;
                }
            });

            // Assert
            Assert.Equal(1, successCount);
        }

        [Fact]
        public void Should_Not_Confirm_Expired_Reservation()
        {
            var (service, repository, item) = CreateTestSetup();

            // Arrange
            service.Reserve(item.Id);

            var reservation = repository.GetByItemId(item.Id).First();

            reservation.ForceExpire(); // or internal set / method

            // Act
            var result = service.Confirm(reservation.Id);

            // Assert
            Assert.Equal(ReservationResult.Expired, result);
            Assert.Equal(ReservationStatus.Expired, reservation.Status);
        }

        [Fact]
        public void Should_Not_Change_Status_When_Already_Confirmed()
        {
            var (service, repository, item) = CreateTestSetup();

            service.Reserve(item.Id);
            var reservation = repository.GetByItemId(item.Id).First();

            service.Confirm(reservation.Id); // first time

            service.Confirm(reservation.Id); // second time

            Assert.Equal(ReservationStatus.Confirmed, reservation.Status);
        }

        [Fact]
        public void Should_Not_Confirm_When_Reservation_Expired()
        {
            var (service, repository, item) = CreateTestSetup();

            service.Reserve(item.Id);
            var reservation = repository.GetByItemId(item.Id).First();

            // simulate expiry
            reservation.ForceExpire();

            service.Confirm(reservation.Id);

            Assert.Equal(ReservationStatus.Expired, reservation.Status);
        }

        [Fact]
        public void Should_Confirm_Only_Target_Reservation()
        {
            var (service, repository, item) = CreateTestSetup(stock: 2);

            service.Reserve(item.Id);
            service.Reserve(item.Id);

            var reservations = repository.GetByItemId(item.Id).ToList();

            var first = reservations[0];
            var second = reservations[1];

            service.Confirm(first.Id);

            Assert.Equal(ReservationStatus.Confirmed, first.Status);
            Assert.Equal(ReservationStatus.Active, second.Status);
        }
    }
}
