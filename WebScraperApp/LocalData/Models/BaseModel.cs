using SQLite;

namespace WebScraperApp.LocalData.Models
{
    public class BaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
    }
}
