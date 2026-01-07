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

        // Aynı ürün depoda varsa getir
        public async Task<StockItem?> GetStockAsync(string productname,  string warehousedestination)
        {
            await InitAsync();

            return await _database!
                .Table<StockItem>()
                .Where(t => t.ProductsName == productname && t.WarehouseDestination == warehousedestination)
                .FirstOrDefaultAsync();
        }
        //----------------------------------------------------------------------------------
        public async Task AddOrUpdateStockAsync(
            string productName,
            string warehouseDestination,
            int quantityChange)
        {
            await InitAsync();

            var existingStock = await GetStockAsync(productName, warehouseDestination);

            if (existingStock == null)
            {
                // İlk kayıt
                if (quantityChange < 0)
                    throw new Exception("Stock cannot be negative.");

                var newItem = new StockItem
                {
                    ProductsName = productName,
                    WarehouseDestination = warehouseDestination,
                    Quantity = quantityChange,
                    CreationDate = DateTime.Now
                };

                await _database!.InsertAsync(newItem);
            }
            else
            {
                int newQuantity = existingStock.Quantity + quantityChange;

                if (newQuantity < 0)
                    throw new Exception("Stock cannot go below zero.");

                existingStock.Quantity = newQuantity;
                await _database!.UpdateAsync(existingStock);
            }
        }


        public async Task<int> GetTotalStockInWarehouseAsync(string warehouseDestination)
        {
            await InitAsync();

            var stocks = await _database!
                .Table<StockItem>()
                .Where(s => s.WarehouseDestination == warehouseDestination)
                .ToListAsync();

            return stocks.Sum(s => s.Quantity);
        }


    }
}
