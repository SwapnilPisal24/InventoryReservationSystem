using IRS.Tests.Fakes;
using Xunit;
using IRS.Application.Interfaces;

namespace IRS.Tests.Unit.Services
{
    public class ReservationServiceTests
    {
        [Fact]
        public void Should_Reserve_When_Stock_Is_Available()
        {
            // Arrange
            var service = new FakeReservationService(); // temporary

            var itemId = Guid.NewGuid();

            // Act
            var result = service.Reserve(itemId);

            // Assert
            Assert.True(result);
        }
    }
}
