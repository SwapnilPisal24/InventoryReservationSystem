using IRS.Domain.Entities;
using IRS.Domain.Enums;
using IRS.Domain.Interfaces;

namespace IRS.Infrastructure.Repositories
{
    public class InMemoryReservationRepository : IReservationRepository
    {
        private readonly List<Reservation> _reservations = new();

        public void Add(Reservation reservation)
        {
            _reservations.Add(reservation);
        }

        public List<Reservation> GetByItemId(Guid itemId)
        {
            return _reservations.Where(r => r.ItemId == itemId).ToList();
        }

        public Reservation? GetById(Guid reservationId)
        {
            return _reservations.FirstOrDefault(r => r.Id == reservationId);
        }

    }
}
