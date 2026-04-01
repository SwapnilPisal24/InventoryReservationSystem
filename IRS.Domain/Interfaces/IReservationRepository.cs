using IRS.Domain.Entities;

namespace IRS.Domain.Interfaces
{
    /// <summary>
    ///  This interface defines the contract for a repository responsible for managing Reservation entities,
    ///  including adding new reservations and retrieving reservations by item ID.
    /// </summary>
    public interface IReservationRepository
    {
        void Add(Reservation reservation);
        List<Reservation> GetByItemId(Guid itemId);
    }
}
