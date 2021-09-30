using System;
using System.IO;
using System.Threading.Tasks;
using WebScraperApp.LocalData.Databases;
using WebScraperApp.LocalData.Entity;
using Xamarin.Forms;

namespace WebScraperApp
{
    public partial class App : Application
    {
        /*
        static TokenDatabaseController tokenDatabase;
        static UserDatabaseController userDatabase;
        static RestService restService;
        */

        static UserDB userDB;

        public static UserDB UserDB
        {
            get
            {
                if (userDB == null)
                {
                    userDB = new UserDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "User.db"));
                }
                return userDB;
            }
        }

        public App()
        {
            InitializeComponent();

            //navigationPage.On<iOS>().SetPrefersLargeTitles(true);
            Task.Run(() => { this.DeleteUser(); });
            MainPage = new Pages.MainPage();
        }

        private async void DeleteUser()
        {
            UserInfo entUser = await UserDB.GetUserAsync(1);
            if (entUser != null)
            {
                await UserDB.DeleteAsync(entUser);
            }
        }


        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        /*
        public static UserDatabaseController UserDatabase
        {
            get
            {
                if (userDatabase == null)
                {
                    userDatabase = new UserDatabaseController();
                }
                return userDatabase;
            }
        }
        public static TokenDatabaseController TokenDatabase
        {
            get
            {
                if (tokenDatabase == null)
                {
                    tokenDatabase = new TokenDatabaseController();
                }
                return tokenDatabase;
            }
        }

        public static RestService RestService
        {
            get
            {
                if (restService == null)
                {
                    restService = new RestService();
                }
                return restService;
            }
        }
        */
    }
}
