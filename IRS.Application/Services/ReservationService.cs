using IRS.Application.Interfaces;
using IRS.Domain.Entities;
using IRS.Domain.Enums;
using IRS.Domain.Interfaces;

namespace IRS.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _repository;
        private readonly InventoryItem _item;

        private readonly object _lock = new();

        public ReservationService(IReservationRepository repository, InventoryItem item)
        {
            _repository = repository;
            _item = item;
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
        public bool Reserve(Guid itemId)
        {
            lock (_lock) // concurrency control
            {
                var reservations = _repository.GetByItemId(itemId);

                int active = reservations.Count(r => r.Status == ReservationStatus.Active);
                int confirmed = reservations.Count(r => r.Status == ReservationStatus.Confirmed);

                int available = _item.TotalStock - active - confirmed;

                if (available <= 0)
                    return false;

                var reservation = new Reservation(itemId, TimeSpan.FromMinutes(2));

                _repository.Add(reservation);

                return true;
            }
        }
    }
}
