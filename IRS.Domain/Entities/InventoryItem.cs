using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRS.Domain.Entities
{
    /// <summary>
    /// InventoryItem represents an item in the inventory with its unique identifier and total stock quantity.
    /// It ensures that the stock cannot be negative and provides a constructor for initializing these properties.
    /// </summary>
    public class InventoryItem
    {
        
        //Private set : to Prevents random modification
        public Guid Id { get; private set; }
        public int TotalStock { get; private set; }

        public InventoryItem(Guid id, int totalStock)
        {
            if (totalStock < 0)
                throw new ArgumentException("Stock cannot be negative");

            Id = id;
            TotalStock = totalStock;
        }
    }
}
