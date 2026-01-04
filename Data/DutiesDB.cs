using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Inventory_Management_System.Models;

namespace Inventory_Management_System.Data
{
    public class DutiesDB
    {
        private SQLiteAsyncConnection? _database;
        
        private async Task InitAsync() // db bağlantısı + tablo oluşturma
        {
            if (_database is not null) // bağlantı daha önce kuruldu mu diye kontrol
                return;// bağlantı kurulduysa kodun kalanı çalışmaz 

            var DbPath = Path.Combine(FileSystem.AppDataDirectory, "duties.db3"); //dosyanın nerede saklanacağını belirler
            _database = new SQLiteAsyncConnection(DbPath);//Belirlenen dosya yolunu kullanarak async bir SQLite bağlantısı açar.
            await _database.CreateTableAsync<DutiesItem>(); // duties item ı db ye dönüştürür. + tablo oluşturulurken uı donmasın diye await kullandım
        }

        public async Task CreateAsync( string Titledb , DateTime DueDatedb) // veriyi kaydetme
        {
            var item = new DutiesItem // bir satırı temsil eden nesne
            {
                Title = Titledb,
                DueDate = DueDatedb,
                IsCompleted = false
            };
            await InitAsync();
            await _database!.InsertAsync(item);
        }

        public async Task CompletionStatusAsync(DutiesItem item)
        {
            await InitAsync();
            item.IsCompleted = !item.IsCompleted;
            await _database!.UpdateAsync(item);
        }

        public async Task<List<DutiesItem>> GetRecentlyCompletedOrNotCompletedAsync()
        {
            await InitAsync();
            return await _database!
                .Table<DutiesItem>()
                .Where(item => (!item.IsCompleted) || (item.IsCompleted && item.DueDate.AddDays(-1) < DateTime.Now))
                .OrderByDescending(item => item.DueDate)
                .ToListAsync();

        }

    }
}
