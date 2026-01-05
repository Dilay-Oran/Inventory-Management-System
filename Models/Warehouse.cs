using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Inventory_Management_System.Models
{
    public class WarehouseItem
    {
        [PrimaryKey,AutoIncrement] 
        public int WarehouseId { get; set; }
        public int Quantity { get; set; } = 0; //fk
        public string WarehouseDestination { get; set; } = string.Empty;
        public int WarehouseCapacity { get; set; }

    }
}
