using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Inventory_Management_System.Models;

namespace Inventory_Management_System.Data
{
    public class WarehouseDB
    {
        private SQLiteAsyncConnection? _database;

        private async Task InitAsync()
        {
            if (_database is not null)
                return;
            var DbPath = Path.Combine(FileSystem.AppDataDirectory, "warehouse.db3");
            _database = new SQLiteAsyncConnection(DbPath);
            await _database.CreateTableAsync<WarehouseItem>();
        }

        public async Task CreateAsync(WarehouseItem item)
        {
            await InitAsync();
            await _database!.InsertAsync(item);
        }

        public async Task CreateAsync( int WarehouseIddb,string Destinationdb,int Capacitydb)
        {

            var item = new WarehouseItem
            {
            WarehouseCapacity = Capacitydb,
            WarehouseDestination = Destinationdb,
            WarehouseId = WarehouseIddb
            };
            await CreateAsync(item);
        }

        public async Task <List<WarehouseItem>> GetAllAsync()
        {
            await InitAsync();
            return await _database!
            .Table<WarehouseItem>()
            .OrderBy(item => item.WarehouseDestination) 
            .ToListAsync();
        }
           





    }
}
