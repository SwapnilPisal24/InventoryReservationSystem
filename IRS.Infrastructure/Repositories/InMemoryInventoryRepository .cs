using IRS.Domain.Entities;
using IRS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRS.Infrastructure.Repositories
{
    public class InMemoryInventoryRepository : IInventoryRepository
    {
        public int GetStock(Guid itemId)
        {
            return 10; // later replace with DB
        }

        public int SetStock(Guid itemId, int stock)
        {
            return stock; // later replace with DB
        }
    }
}
