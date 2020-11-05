using SQLite;

namespace WebScraperApp.Data.Interface
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
