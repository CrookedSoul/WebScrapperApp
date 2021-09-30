using SQLite;
using System.IO;
using Xamarin.Forms;

namespace WebScraperApp.LocalData.Entity
{
    [Table("User")]
    public class UserInfo : BaseInfo
    {
        [Column("UserName")]
        public string UserName { get; set; }
        [Column("ProfileImage")]
        public byte[] ProfileImage { get; set; }
        public ImageSource ProfileImageSource
        {
            get
            {
                return ImageSource.FromStream(() => new MemoryStream(ProfileImage)); ;
            }
        }
    }
}
