using SQLite;

namespace WebScraperApp.LocalData.Entity
{
    public class BaseInfo
    {
        [Column("ID")]
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
    }
}
