using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRS.Domain.Enums
{
    /// <summary>
    /// ReservationStatus represents the various states a reservation can be in, such as Active, Confirmed, Cancelled, and Expired.
    /// </summary>
    public enum ReservationStatus
    {
        // Holding stock
        Active,

        // Purchased
        Confirmed,

        // Purchased
        Cancelled,

        // User cancelled
        Expired
    }
}
