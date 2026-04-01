using IRS.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRS.Tests.Fakes
{
    public class FakeReservationService : IReservationService
    {
        public bool Reserve(Guid itemId)
        {
            return true; // temporary just to pass test
        }
    }
}
