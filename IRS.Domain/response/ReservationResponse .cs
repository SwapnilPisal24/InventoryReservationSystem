using IRS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRS.Domain.response
{
    public class ReservationResponse
    {
        public Guid? ReservationId { get; set; }
        public ReservationResult Result { get; set; }
    }
}
