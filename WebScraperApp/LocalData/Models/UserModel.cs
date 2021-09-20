namespace WebScraperApp.LocalData.Models
{
    public class UserModel : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImageUrl { get; set; }
        public byte ProfileImage { get; set; }
    }
}
