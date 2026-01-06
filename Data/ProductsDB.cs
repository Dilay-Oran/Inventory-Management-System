using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Inventory_Management_System.Models;

namespace Inventory_Management_System.Data
{
    public class ProductsDB
    {
        private SQLiteAsyncConnection? _database;

        private async Task InitAsync()
        {
            if (_database is not null)
                return;
            var DbPath = Path.Combine(FileSystem.AppDataDirectory, "products.db3");
            _database = new SQLiteAsyncConnection(DbPath);
            await _database.CreateTableAsync<ProductsItem>();
        }

        public async Task CreateAsync(ProductsItem item)
        {
            await InitAsync();
            await _database!.InsertAsync(item);
        }

        public async Task CreateAsync(int productsid , string productsname)
        {

            var item = new ProductsItem
            {
                ProductsName = productsname,
            };
            await CreateAsync(item);
        }

        public async Task<List<ProductsItem>> GetAllAsync()
        {
            await InitAsync();
            return await _database!
            .Table<ProductsItem>()
            .OrderBy(item => item.ProductsId)
            .ToListAsync();
        }
        public async Task DeleteAsync(ProductsItem  item)
        {
            await InitAsync();
            await _database!.DeleteAsync(item);
        }

    }
}
