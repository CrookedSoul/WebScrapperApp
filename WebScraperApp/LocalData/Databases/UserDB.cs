using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebScraperApp.LocalData.Entity;

namespace WebScraperApp.LocalData.Databases
{
    public class UserDB
    {
        readonly SQLiteAsyncConnection database;

        public UserDB(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);

            database.CreateTableAsync<UserInfo>().Wait();
            database.CreateTableAsync<UserBookmarkInfo>().Wait();
        }

        public Task<List<UserInfo>> GetAsync()
        {
            //Get all notes.
            return database.Table<UserInfo>().ToListAsync();
        }

        public Task<UserInfo> GetUserAsync(int id)
        {
            // Get a specific column.
            return database.Table<UserInfo>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveAsync(UserInfo ent)
        {
            if (ent.ID != 0)
            {
                // Update an existing column.
                return database.UpdateAsync(ent);
            }
            else
            {
                // Save a new column.
                return database.InsertAsync(ent);
            }
        }

        public Task<int> DeleteAsync(UserInfo ent)
        {
            // Delete a column.
            return database.DeleteAsync(ent);
        }
    }
}
