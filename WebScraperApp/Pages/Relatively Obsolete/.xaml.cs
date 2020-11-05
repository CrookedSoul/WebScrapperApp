using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using WebScraperApp.Model.Novel;
using WebScraperApp.Pages;
using WebScraperApp.Pages.Novel;
using WebScraperApp.Scrapers.NovelFull;
using Xamarin.Forms;

namespace WebScraperApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        NovelListScraper scraper;
        int page = 1;
        public MainPage()
        {
            InitializeComponent();
            scraper = new NovelListScraper();

            // The colors
            scraper.BackgroundColor = Color.FromHex("0e1821");
            scraper.BorderColor = Color.FromHex("fffafa");
            scraper.TextColor = Color.FromHex("62c6f5");

            this.init();
        }

        private async void init()
        {
            NovelLayout.BackgroundColor = scraper.BackgroundColor;

            await Task.Run(() =>
            {
                scraper.ScrapeNovelListData(page.ToString());
                List<ItemModel> list = scraper.NovelList;
                foreach (ItemModel item in list)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        this.AddNovels(item);
                    });
                }
            });
        }

        //private void NovelList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //     ListModel ChosenNovel = (ListModel)e.CurrentSelection[0];

        //     Navigation.PushAsync(new ViewPage(ChosenNovel.NovelUrl));
        //     //Navigation.PushAsync(new NovelPage(ChosenNovel.NovelUrl));
        //}

        //private void NovelList_RemainingItemsThresholdReached(object sender, EventArgs e)
        //{
        //    page += 1;
        //    scraper.ScrapeNovelListData(page.ToString());
        //    NovelList.ItemsSource = scraper.NovelList;
        //}
        private void AddNovels(ItemModel model)
        {
            //Button button = new Button
            //{
            //    Text = model.Title,
            //    Margin = new Thickness(0, 0, 5, 5),
            //    FontSize = 12,
            //    Padding = 0,
            //    TextColor = model.TextColor,
            //    ImageSource = model.ImageURL,
            //    //BorderColor = model.TagTextColor,
            //    CornerRadius = 15,
            //    BorderWidth = 2,
            //    HeightRequest = 250,
            //    WidthRequest = 125,
            //    HorizontalOptions = LayoutOptions.Start,
            //    CommandParameter = model.NovelUrl
            //};
            //button.Pressed += this.SelectedNovel;

            Grid Card = new Grid
            {
                BackgroundColor = Color.Transparent,
                Margin = 5,
                HeightRequest = 175,
                WidthRequest = 125,
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(125)},
                    new RowDefinition { Height = new GridLength(50) },
                },
            };
            var tapGestureRecognizer = new TapGestureRecognizer {
                CommandParameter = model.NovelUrl,
            };
            Image image = new Image
            {
                Aspect = Aspect.AspectFill,
                Margin = new Thickness(25, 5, 25, 5),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Source = model.ImageURL,
            };
            tapGestureRecognizer.Tapped += this.SelectedNovel;
            image.GestureRecognizers.Add(tapGestureRecognizer);
            Label title = new Label
            {
                Text = model.Title,
                TextColor = model.TextColor,
                MaxLines = 2,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Padding = new Thickness(5, 5, 5, 5),
            };
            Card.Children.Add(image, 0, 0);
            Card.Children.Add(title, 0, 1);

            NovelLayout.Children.Add(Card);
        }

        private void SelectedNovel(object sender, System.EventArgs e)
        {
            string url = /*((Button)sender).CommandParameter.ToString()*/ ((TappedEventArgs)e).Parameter.ToString();//NovelFullConst.SearchByTag + ((Button)sender).CommandParameter;

            Navigation.PushAsync(new ViewPage(url));
        }
        private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {

        }
    }
}
