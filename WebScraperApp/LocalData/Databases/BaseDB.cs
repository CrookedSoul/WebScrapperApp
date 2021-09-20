using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using WebScraperApp.LocalData.Models;

namespace WebScraperApp.LocalData.Databases
{
    public class BaseDB<TModel>
        where TModel: BaseModel, new()
    {
        readonly SQLiteAsyncConnection database;

        public BaseDB(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<TModel>().Wait();
        }

        public Task<List<TModel>> GetAsync()
        {
            //Get all notes.
            return database.Table<TModel>().ToListAsync();
        }

        public Task<TModel> GetUserAsync(int id)
        {
            // Get a specific column.
            return database.Table<TModel>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveAsync(TModel model)
        {
            if (model.ID != 0)
            {
                // Update an existing column.
                return database.UpdateAsync(model);
            }
            else
            {
                // Save a new column.
                return database.InsertAsync(model);
            }
        }

        public Task<int> DeleteAsync(TModel model)
        {
            // Delete a column.
            return database.DeleteAsync(model);
        }
    }
}
