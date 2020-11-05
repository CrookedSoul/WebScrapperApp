﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebScraperApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WebScraperApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        ViewCell lastCell;
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        public MenuPage()
        {
            this.WidthRequest = 10;
            InitializeComponent();

            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.MyLibrary, Title="My Library" },
                new HomeMenuItem {Id = MenuItemType.Browse, Title="Browse" }
            };

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };

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

        protected override void OnAppearing()
        {
            base.OnAppearing();

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