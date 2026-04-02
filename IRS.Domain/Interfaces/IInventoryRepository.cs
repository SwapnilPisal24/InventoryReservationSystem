using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRS.Domain.Interfaces
{
    public interface IInventoryRepository
    {
        int GetStock(Guid itemId);
        int SetStock(Guid itemId, int stock);
    }
}
