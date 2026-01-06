using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory_Management_System.Models;
using SQLite;

namespace Inventory_Management_System.Data
{
    public class StockDB
    {
        private SQLiteAsyncConnection? _database;

        private async Task InitAsync()
        {
            if (_database is not null)
                return;
            var DbPath = Path.Combine(FileSystem.AppDataDirectory, "stock.db3");
            _database = new SQLiteAsyncConnection(DbPath);
            await _database.CreateTableAsync<StockItem>();
        }

        public async Task CreateAsync(StockItem item)
        {
            await InitAsync();
            await _database!.InsertAsync(item);
        }

        public async Task CreateAsync(string productsname, string warehousedestination, int quantity)
        {
            var item = new StockItem
            {
                ProductsName = productsname,
                WarehouseDestination = warehousedestination,
                Quantity = quantity,
                CreationDate = DateTime.Now
            };

            await CreateAsync(item);
        }


        public async Task<List<StockItem>> GetAllAsync()
        {
            await InitAsync();
            return await _database!
            .Table<StockItem>()
            .OrderBy(item => item.CreationDate)
            .ToListAsync();
        }
        public async Task DeleteAsync(StockItem item)
        {
            await InitAsync();
            await _database!.DeleteAsync(item);
        }
    }
}
