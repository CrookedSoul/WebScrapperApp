using SQLite;
using WebScraperApp.Data.Interface;
using WebScraperApp.Data.Models;
using Xamarin.Forms;

namespace WebScraperApp.Data.Controller
{
    public class UserDatabaseController
    {
        static object locker = new object();

        SQLiteConnection database;

        // Create a table if it doesnt exist
        public UserDatabaseController()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
            database.CreateTable<User>();
        }

        public User GetUser()
        {
            lock (locker)
            {
                if (database.Table<User>().Count() == 0)
                {
                    return null;
                }
                else
                {
                    return database.Table<User>().First();
                }
            }
        }

        public int SaveUser(User user)
        {
            lock (locker)
            {
                if (user.ID != 0)
                {
                    database.Update(user);
                    return user.ID;
                }
                else
                {
                    return database.Insert(user);
                }
            }
        }
        public int DeleteUser(int id)
        {
            lock (locker)
            {
                return database.Delete<User>(id);
            }
        }
    }
}
