using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebScraperApp.LocalData.Entity;
using WebScraperApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WebScraperApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        UserInfo entUser = new UserInfo();
        public UserInfo userModel
        {
            get { ProfileMenuGrid.BindingContext = entUser;
                  return entUser; }
            set { entUser = value; 
                  ProfileMenuGrid.BindingContext = entUser;  }
        }

        ViewCell lastCell;
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        public MenuPage()
        {
            this.WidthRequest = 10;
            InitializeComponent();

            // Ovde iteme oznacimo
            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.MyLibrary, Title="My Library" },
                new HomeMenuItem {Id = MenuItemType.Browse, Title="Browse" }
            };

            // Ovde binfujemo te iteme na Meni
            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];

            // Idemo tokom klika na selektani item
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };

            // Color change kada kliknut
            IEnumerable<PropertyInfo> pInfos = (ListViewMenu as ItemsView<Cell>).GetType().GetRuntimeProperties();
            var templatedItems = pInfos.FirstOrDefault(info => info.Name == "TemplatedItems");
            if (templatedItems != null)
            {
                var cells = templatedItems.GetValue(ListViewMenu);
                var x = 0;
                foreach (ViewCell cell in cells as Xamarin.Forms.ITemplatedItemsList<Xamarin.Forms.Cell>)
                {
                    if (cell.BindingContext != null && cell.BindingContext is HomeMenuItem && x < 1)
                    {
                        cell.View.BackgroundColor = (Color)Application.Current.Resources["ClickedCellColor"];
                        x++;


                        //HomeMenuItem target = (cell.View as Grid).Children.OfType<HomeMenuItem>().FirstOrDefault();
                        //if (target != null)
                        //{
                        //do whatever you want to do with your item... 
                        //}
                    }

                }
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //Get All Persons  
            UserInfo ent = await App.UserDB.GetUserAsync(1);
            if (ent != null)
            {
                userModel = ent;
            }
            else
            {
                // Set defaults
                ent = new UserInfo();
                ent.UserName = "Unknown";
                userModel = ent;

                // No cells are clicked
                IEnumerable<PropertyInfo> pInfos = (ListViewMenu as ItemsView<Cell>).GetType().GetRuntimeProperties();
                var templatedItems = pInfos.FirstOrDefault(info => info.Name == "TemplatedItems");
                if (templatedItems != null)
                {
                    var cells = templatedItems.GetValue(ListViewMenu);
                    foreach (ViewCell cell in cells as Xamarin.Forms.ITemplatedItemsList<Xamarin.Forms.Cell>)
                    {
                        cell.View.BackgroundColor = (Color)Application.Current.Resources["BackgroundColor"];
                    }
                }

                var id = (int)(MenuItemType.UserProfile);
                await RootPage.NavigateFromMenu(id);
            }
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            if (lastCell != null)
            {
                lastCell.View.BackgroundColor = (Color)Application.Current.Resources["BackgroundColor"];
            }
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = (Color)Application.Current.Resources["ClickedCellColor"];
                lastCell = viewCell;
            }
        }
    }
}