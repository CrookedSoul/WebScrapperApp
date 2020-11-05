using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Stormlion.SNavigation;
using WebScraperApp.Model;
using System.ComponentModel;

namespace WebScraperApp.Pages
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, SNavigationPage> MenuPages = new Dictionary<int, SNavigationPage>();
        public MainPage()
        {

            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;

            Detail = new SNavigationPage(new Novel.Catalog.ListPage());
        }

        public async Task NavigateFromMenu(int id)
        {
            //if (!MenuPages.ContainsKey(id))
            //{
            //    switch (id)
            //    {
            //        case (int)MenuItemType.MyLibrary:
            //            // This is not right it should be another page but good fgor testing purpose
            //            MenuPages.Add(id, new SNavigationPage(new Novel.Catalog.ListPage()));
            //            break;
            //        case (int)MenuItemType.Browse:
            //            // This wil be list page with diff Websites
            //            MenuPages.Add(id, new SNavigationPage(new Novel.Catalog.ViewPage(null)));
            //            break;
            //    }
            //}

            //var newPage = MenuPages[id];

            //if (newPage != null && Detail != newPage)
            //{
            //    Detail = newPage;

            //    if (Device.RuntimePlatform == Device.Android)
            //        await Task.Delay(100);

            //    IsPresented = false;
            //}   
            switch (id)
            {
                case (int)MenuItemType.MyLibrary:
                    // This is not right it should be another page but good fgor testing purpose
                    Detail = new SNavigationPage(new Novel.Catalog.ListPage());
                    break;
                case (int)MenuItemType.Browse:
                    // This wil be list page with diff Websites
                    Detail = new SNavigationPage(new Novel.Catalog.ViewPage(null));
                    break;
            }

            if (Device.RuntimePlatform == Device.Android)
                await Task.Delay(100);

            IsPresented = false;
        }
    }
    //MasterPage masterPage;
    //public MasterPage()
    //{
    //    Detail = new SNavigationPage(new Pages.Novel.Catalog.ViewPage(null)); 

    //    InitializeComponent();
    //}

    //private void Catalog_Clicked(object sender, EventArgs e)
    //{
    //    Detail = new SNavigationPage(new Pages.Novel.Catalog.ViewPage(null));
    //}

    //private void List_Clicked(object sender, EventArgs e)
    //{
    //    Detail = new SNavigationPage(new Pages.Novel.Catalog.ListPage());
    //}
}