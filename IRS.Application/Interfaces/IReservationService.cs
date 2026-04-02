using IRS.Domain.Enums;
using IRS.Domain.response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRS.Application.Interfaces
{
    /// <summary>
    /// IReservationService defines the contract for a service responsible for handling reservation operations, such as reserving an inventory item by its unique identifier.
    /// </summary>
    public interface IReservationService
    {
        ReservationResponse Reserve(Guid itemId);
        ReservationResult Confirm(Guid reservationId);      
    }
}
