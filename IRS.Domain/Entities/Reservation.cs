using IRS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRS.Domain.Entities
{
    /// <summary>
    /// Reservation represents a reservation of an inventory item, including its unique identifier, the associated item ID, expiry time, and current status.
    /// </summary>
    public class Reservation
    {
        public Guid Id { get; private set; }
        public Guid ItemId { get; private set; }
        public DateTime ExpiryTime { get; private set; }
        public ReservationStatus Status { get; private set; }

        // Constructor initializes a new reservation with the specified item ID and hold duration, setting the status to Active and calculating the expiry time based on the current time.
        public Reservation(Guid itemId, TimeSpan holdDuration)
        {
            Id = Guid.NewGuid();
            ItemId = itemId;
            ExpiryTime = DateTime.UtcNow.Add(holdDuration);
            Status = ReservationStatus.Active;
        }

        // Confirm method changes the status of the reservation to Confirmed if it is currently Active, otherwise it throws an exception indicating that only active reservations can be confirmed.
        public void Confirm()
        {
            if (Status != ReservationStatus.Active)
                throw new InvalidOperationException("Only active reservations can be confirmed");

            Status = ReservationStatus.Confirmed;
        }

        // Cancel method changes the status of the reservation to Cancelled if it is currently Active, otherwise it throws an exception indicating that only active reservations can be cancelled.
        public void Cancel()
        {
            if (Status != ReservationStatus.Active)
                throw new InvalidOperationException("Only active reservations can be cancelled");

            Status = ReservationStatus.Cancelled;
        }

        // Expire method changes the status of the reservation to Expired if it is currently Active, allowing for the reservation to be marked as expired when the hold duration has passed.
        public void Expire()
        {
            if (Status == ReservationStatus.Active)
                Status = ReservationStatus.Expired;
        }
    }
}
