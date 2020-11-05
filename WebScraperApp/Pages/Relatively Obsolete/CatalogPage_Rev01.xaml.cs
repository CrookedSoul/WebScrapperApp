using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraperApp.Model;
using WebScraperApp.Model.Novel;
using WebScraperApp.Model.SiteConst;
using WebScraperApp.Pages.Novel;
using WebScraperApp.Scrapers.NovelFull.Novel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WebScraperApp.Pages.Relatively_Obsolete
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatalogPage_Rev01 : ContentPage
    {
        private int currentContext = SearchModel.CatalogOpen;
        ListScraper novelList;
        int page = 1;
        bool isBusy = false;

        string value = "";
        public CatalogPage_Rev01(string tag)
        {
            InitializeComponent();
            novelList = new ListScraper();

            // The colors
            novelList.BackgroundColor = Color.FromHex("0e1821");
            novelList.BorderColor = Color.FromHex("fffafa");
            novelList.TextColor = Color.FromHex("62c6f5");

            if (String.IsNullOrEmpty(tag))
            {
                currentContext = SearchModel.CatalogOpen;
                novelList.ScrapeNovelListData(page.ToString(), null, currentContext);
            }
            else
            {
                currentContext = SearchModel.SearchByGenre;
                value = tag;
                novelList.ScrapeNovelListData(page.ToString(), tag, currentContext);
            }
            BindingContext = novelList;
            NovelList.ItemsSource = novelList.List;
        }

        private void NovelList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemModel ChosenNovel = (ItemModel)e.CurrentSelection[0];

            Navigation.PushAsync(new ViewPage(ChosenNovel.NovelUrl));
            //Navigation.PushAsync(new NovelPage(ChosenNovel.NovelUrl));
        }

        private void NovelList_RemainingItemsThresholdReached(object sender, EventArgs e)
        {
            page += 1;

            if (currentContext == SearchModel.CatalogOpen && String.IsNullOrEmpty(value))
            {
                novelList.ScrapeNovelListData(page.ToString(), null, currentContext);
            }
            else if (currentContext == SearchModel.SearchByName && !String.IsNullOrEmpty(value))
            {
                novelList.ScrapeNovelListData(page.ToString(), value, currentContext);
            }
            else if (currentContext == SearchModel.SearchByGenre && !String.IsNullOrEmpty(value))
            {
                novelList.ScrapeNovelListData(page.ToString(), value, currentContext);
            }
            NovelList.ItemsSource = novelList.List;
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            page = 1;
            if (!isBusy)
            {
                await Task.Run(() =>
                {
                    isBusy = true;
                    novelList.ClearCache();
                    if (!String.IsNullOrWhiteSpace(e.NewTextValue))
                    {
                        currentContext = SearchModel.SearchByName;
                        value = e.NewTextValue;
                        novelList.ScrapeNovelListData(page.ToString(), e.NewTextValue, currentContext);
                    }
                    else
                    {
                        currentContext = SearchModel.CatalogOpen;
                        value = "";
                        novelList.ScrapeNovelListData(page.ToString(), null, currentContext);
                    }
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        BindingContext = novelList;
                        NovelList.ItemsSource = novelList.List;
                    });
                    isBusy = false;
                });
            }
        }
    }
}