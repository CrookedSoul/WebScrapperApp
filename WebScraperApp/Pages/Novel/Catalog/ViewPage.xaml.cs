﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebScraperApp.Model;
using WebScraperApp.Model.Novel;
using WebScraperApp.Model.SiteConst;
using WebScraperApp.Scrapers.NovelFull.Novel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WebScraperApp.Pages.Novel.Catalog
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewPage : ContentPage
    {
        private int currentContext = SearchModel.CatalogOpen;
        ListScraper scraper;
        List<ItemModel> list = new List<ItemModel>();
        List<ItemModel> previousList = new List<ItemModel>();
        int page = 1;
        bool isBusy = false;
        double CardWidth = -1;
        double CardHeight = -1;
        double ImageRow = -1;
        double LabelRow = -1;

        string value = "";
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

        public ViewPage(string tag)
        {
            InitializeComponent();
            scraper = new ListScraper();

            
            // The colors
            scraper.BackgroundColor = (Color)Application.Current.Resources["BackgroundColor"];
            scraper.BorderColor = (Color)Application.Current.Resources["BorderColor"];
            scraper.TextColor = (Color)Application.Current.Resources["TextColor"];

            this.init(tag);
        }
        private async void init(string tag)
        {
            NovelLayout.BackgroundColor = scraper.BackgroundColor;

            await Task.Run(() =>
            {
                if (String.IsNullOrEmpty(tag))
                {
                    currentContext = SearchModel.CatalogOpen;
                    scraper.ScrapeNovelListData(page.ToString(), null, currentContext);
                    list = scraper.List;
                }
                else
                {
                    currentContext = SearchModel.SearchByGenre;
                    value = tag;
                    scraper.ScrapeNovelListData(page.ToString(), tag, currentContext);
                    list = scraper.List;
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.AddNovels(list);
                });
            });
        }

        private async void AddNovels(List<ItemModel> model)
        {
            if (CardWidth < 1) CardWidth = Application.Current.MainPage.Width * 0.30;
            if (CardHeight < 1) CardHeight = Application.Current.MainPage.Height * 0.30;
            if (ImageRow < 1) ImageRow = CardHeight * 0.75;
            if (LabelRow < 1) LabelRow = CardHeight * 0.25;


            foreach (ItemModel item in list)
            {
                Grid Card = new Grid
                {
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    BackgroundColor = Color.Transparent,
                    Margin = new Thickness(5, 5, 5, 0),
                    HeightRequest = CardHeight,
                    WidthRequest = CardWidth,
                    RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(ImageRow) },
                    new RowDefinition { Height = new GridLength(LabelRow) },
                },
                };
                var tapGestureRecognizer = new TapGestureRecognizer
                {
                    CommandParameter = item.NovelUrl,
                };
                Image image = new Image
                {
                    Aspect = Aspect.AspectFit,
                    BackgroundColor = Color.Black,
                    Margin = new Thickness(10, 0, 10, 0),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Source = item.ImageURL,
                    //Source = "https://novelfull.com/uploads/thumbs/library-of-heavens-path-d6292facbc-2239c49aee6b961904acf173b7e4602a.jpg" 
                };
                tapGestureRecognizer.Tapped += this.SelectedNovel;
                image.GestureRecognizers.Add(tapGestureRecognizer);
                Label title = new Label
                {
                    Text = item.Title,
                    FontSize = 12,
                    TextColor = item.TextColor,
                    MaxLines = 2,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(5, 0, 5, 5),
                };
                Card.Children.Add(image, 0, 0);
                Card.Children.Add(title, 0, 1);

                NovelLayout.Children.Add(Card);
                await Card.ScaleTo(1.2, 100);
                await Card.ScaleTo(1, 100, Easing.SpringOut);
            }
        }

        private async void SelectedNovel(object sender, System.EventArgs e)
        {
            string url = ((TappedEventArgs)e).Parameter.ToString();
            Image card = (Image)sender;
            await card.ScaleTo(1.2, 100);
            await card.ScaleTo(1, 100, Easing.SpringOut);

            await Navigation.PushAsync(new Novel.ViewPage(url));
        }

        private void NovelList_RemainingItemsThresholdReached(object sender, EventArgs e)
        {
            page += 1;

            scraper.ClearCache();
            if (currentContext == SearchModel.CatalogOpen && String.IsNullOrEmpty(value))
            {
                scraper.ScrapeNovelListData(page.ToString(), null, currentContext);
            }
            else if (currentContext == SearchModel.SearchByName && !String.IsNullOrEmpty(value))
            {
                scraper.ScrapeNovelListData(page.ToString(), value, currentContext);
            }
            else if (currentContext == SearchModel.SearchByGenre && !String.IsNullOrEmpty(value))
            {
                scraper.ScrapeNovelListData(page.ToString(), value, currentContext);
            }
            list = scraper.List;
            Device.BeginInvokeOnMainThread(() =>
            {
                this.AddNovels(list);
            });
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            page = 1;
            if (!isBusy)
            {
                await Task.Run(() =>
                {
                    isBusy = true;
                    scraper.ClearCache();
                    if (!String.IsNullOrWhiteSpace(e.NewTextValue))
                    {
                        currentContext = SearchModel.SearchByName;
                        value = e.NewTextValue;
                        scraper.ScrapeNovelListData(page.ToString(), e.NewTextValue, currentContext);
                        list = scraper.List;
                    }
                    else
                    {
                        currentContext = SearchModel.CatalogOpen;
                        value = "";
                        scraper.ScrapeNovelListData(page.ToString(), null, currentContext);
                        list = scraper.List;
                    }
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        NovelLayout.Children.Clear();
                        this.AddNovels(list);
                    });


                    isBusy = false;
                });
            }
        }

        private async void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            ScrollView scrollView = sender as ScrollView;
            double scrollingSpace = scrollView.ContentSize.Height - scrollView.Height;

            double CardHeight = 175;
            if (scrollingSpace <= e.ScrollY + (CardHeight * 3))
            {
                if (!isBusy)
                {
                    await Task.Run(() =>
                    {
                        page += 1;

                        isBusy = true;
                        previousList = list;
                        scraper.ClearCache();
                        if (currentContext == SearchModel.CatalogOpen && String.IsNullOrEmpty(value))
                        {
                            scraper.ScrapeNovelListData(page.ToString(), null, currentContext);
                        }
                        else if (currentContext == SearchModel.SearchByName && !String.IsNullOrEmpty(value))
                        {
                            scraper.ScrapeNovelListData(page.ToString(), value, currentContext);
                        }
                        else if (currentContext == SearchModel.SearchByGenre && !String.IsNullOrEmpty(value))
                        {
                            scraper.ScrapeNovelListData(page.ToString(), value, currentContext);
                        }
                    })
                    .ContinueWith(addTheNovels =>
                    {
                        if (addTheNovels.Status == TaskStatus.RanToCompletion)
                        {
                            list = scraper.List;
                            // Check if its the previous page, if it is do not add novels i think... i forgot
                            if ((previousList[0].NovelUrl != list[0].NovelUrl) &&
                               (previousList[0].Title != list[0].Title) &&
                               (previousList[0].Author != list[0].Author))
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    this.AddNovels(list);
                                });
                            }
                            isBusy = false;
                        }
                        else if (addTheNovels.Status == TaskStatus.Canceled)
                        {
                            isBusy = false;
                        };
                    }, TaskContinuationOptions.OnlyOnRanToCompletion);
                }
            }// Touched bottom // Do the things you want to do
        }
    }
}