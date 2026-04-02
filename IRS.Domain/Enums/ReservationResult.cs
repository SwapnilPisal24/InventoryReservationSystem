using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRS.Domain.Enums
{
    public enum ReservationResult
    {
        // Indicates that the reservation was successful and the stock has been reserved.
        Success,

        // Indicates that the reservation failed because there was not enough stock available to fulfill the reservation request.
        OutOfStock,

        // Indicates that the reservation failed because the reservation has already been confirmed and cannot be reserved again.
        AlreadyConfirmed,

        // Indicates that the reservation failed because it has already been cancelled and cannot be reserved again.
        Expired,

        // Indicates that the reservation failed because it has been cancelled by the user and cannot be reserved again.
        Cancelled,

        // Indicates that the reservation failed because the item associated with the reservation was not found in the inventory.
        NotFound,

        // Indicates that the reservation failed due to an unexpected error or issue that prevented the reservation from being completed successfully.
        Failed,

    }
}
