using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Inventory_Management_System.Models
{
    public class StockItem
    {
        [PrimaryKey,AutoIncrement]
        public int StockId { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public int Quantity { get; set; }
        public string WarehouseDestination { get; set; } //fk
        public string ProductsName { get; set; } = string.Empty; //fk

    }
}
