using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WebScraperApp.Pages.Novel.Catalog
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPage : ContentPage
    {
        public ListPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            (Application.Current.MainPage as MasterDetailPage).IsGestureEnabled = true;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            (Application.Current.MainPage as MasterDetailPage).IsGestureEnabled = false;
        }


        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}