    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;


namespace Inventory_Management_System.Models
{
   public class DutiesItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Title { get; set; } = string.Empty; // not nullable and default value is empty
        public DateTime DueDate { get; set; }

        public bool IsCompleted { get; set; }

    }
}
