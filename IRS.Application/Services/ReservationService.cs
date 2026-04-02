using IRS.Application.Interfaces;
using IRS.Domain.Entities;
using IRS.Domain.Enums;
using IRS.Domain.Interfaces;
using System.Collections.Concurrent;

namespace IRS.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _repository;
        private readonly InventoryItem _item;

        // Using a static ConcurrentDictionary to manage locks for each item ID.
        // This allows us to have a separate lock for each item, preventing contention between different items while still ensuring thread safety when reserving stock for the same item.
        private static readonly ConcurrentDictionary<Guid, object> _locks = new();

        //private readonly object _lock = new();

        public ReservationService(IReservationRepository repository, InventoryItem item)
        {
            _repository = repository;
            _item = item;
        }

        /// <summary>
        ///  This method is responsible for expiring reservations that have passed their expiry time. It retrieves all reservations for a given item ID and checks if any active reservations have expired. If so, it updates their status to expired. This ensures that expired reservations do not count towards available stock when checking for new reservations.
        /// </summary>
        /// <param name="itemId"></param>
        private void ExpireReservations(Guid itemId)
        {
            var reservations = _repository.GetByItemId(itemId);

            foreach (var r in reservations)
            {
                if (r.Status == ReservationStatus.Active &&
                    r.ExpiryTime <= DateTime.UtcNow)
                {
                    r.Expire(); // here we are updating the status of the reservation to expired if it is active and has passed its expiry time. This ensures that expired reservations are properly marked and do not count towards available stock when checking for reservations.
                }
            }
        }


        /// <summary>
        /// Why lock ? 
        /// without lock  -> 
        /// Thread A -> see 1 stock
        /// Thread B -> see 1 stock
        /// Both reserved -> overselling 

        /// Without lock ->
        /// Thread A -> Completes    
        /// Thread B -> sees updated stock -> fails to reserve -> no overselling 

        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public ReservationResult Reserve(Guid itemId)
        {
            var lockObj = _locks.GetOrAdd(itemId, _ => new object());

            lock (lockObj)
            {
                var reservations = _repository.GetByItemId(itemId);

                if (CalculateAvailableStock(reservations) <= 0)
                    return ReservationResult.OutOfStock;

                var reservation = new Reservation(itemId, TimeSpan.FromMinutes(2));

                _repository.Add(reservation);

                return ReservationResult.Success;
            }
        }

        public ReservationResult Confirm(Guid reservationId)
        {
            var reservation = _repository
                .GetByItemId(_item.Id)
                .FirstOrDefault(r => r.Id == reservationId);

            if (reservation == null)
                return ReservationResult.NotFound; 

            return reservation.Confirm(); 
        }

        private int CalculateAvailableStock(List<Reservation> reservations)
        {
            int active = reservations.Count(r => r.Status == ReservationStatus.Active);
            int confirmed = reservations.Count(r => r.Status == ReservationStatus.Confirmed);

            return _item.TotalStock - active - confirmed;
        }
    }
}
