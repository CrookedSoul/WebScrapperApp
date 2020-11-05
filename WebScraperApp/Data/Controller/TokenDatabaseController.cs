using SQLite;
using WebScraperApp.Data.Interface;
using WebScraperApp.Data.Models;
using Xamarin.Forms;

namespace WebScraperApp.Data.Controller
{
    public class TokenDatabaseController
    {        static object locker = new object();

        SQLiteConnection database;

        // Create a table if it doesnt exist
        public TokenDatabaseController()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
            database.CreateTable<Token>();
        }

        public Token GetToken()
        {
            lock (locker)
            {
                if (database.Table<Token>().Count() == 0)
                {
                    return null;
                }
                else
                {
                    return database.Table<Token>().First();
                }
            }
        }

        public int SaveToken(Token token)
        {
            lock (locker)
            {
                if (token.ID != 0)
                {
                    database.Update(token);
                    return token.ID;
                }
                else
                {
                    return database.Insert(token);
                }
            }
        }
        public int DeleteToken(int id)
        {
            lock (locker)
            {
                return database.Delete<Token>(id);
            }
        }
    }
}
