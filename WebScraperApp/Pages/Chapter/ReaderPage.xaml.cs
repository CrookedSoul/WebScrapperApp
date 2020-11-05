using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraperApp.Model.Chapter;
using WebScraperApp.Scrapers.NovelFull.Chapter;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WebScraperApp.Pages.Chapter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReaderPage : ContentPage
    {
        ViewCell lastCell;
        ContentScraper scraper;
        string chapterUrl;
        // ChapterList for the navigation
        List<ItemModel> chapterList;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //this.InitializeChapterList(chapterList);
        }
        public ReaderPage(string url, List<ItemModel> chapters)
        {
            InitializeComponent();

            chapterList = chapters;
            ChapterList.ItemsSource = chapterList;
            this.init(url);
        }
        private void init(string url)
        {
            scraper = new ContentScraper();
            BindingContext = scraper;

            // Get parameters 
            ParameterModel paramModel = new ParameterModel();
            if (paramModel.FontSize == null) paramModel.FontSize = ChapterName.FontSize;
            chapterUrl = url;
            paramModel.ChapterUrl = chapterUrl;

            // Get data
            scraper.ScrapeChapterData(paramModel);
            // bind the data
            ViewModel chapter = scraper.Chapter;

            ChapterName.Text = chapter.Name;
            ContentLabel.Text = chapter.Content;
            overlay.IsVisible = false;
            PageContentGrid.IsEnabled = true;
            ContentScroll.ScrollToAsync(0, 0, false);
        }
        private void PreviousChapter_Tapped(object sender, EventArgs e)
        {
            this.AnimateButton(PreviousChapterImage);
            string previousChapter = null;
            if (chapterList.Exists(x => x.CurrentChapterUrl == chapterUrl))
            {
                previousChapter = chapterList.First(x => x.CurrentChapterUrl == chapterUrl).PreviousChapterUrl;
            }
            if (!String.IsNullOrEmpty(previousChapter))
            {
                this.init(previousChapter);
            }
        }

        private void NextChapter_Tapped(object sender, EventArgs e)
        {
            this.AnimateButton(NextChapterImage);
            string nextChapter = null;
            if (chapterList.Exists(x => x.CurrentChapterUrl == chapterUrl))
            {
                nextChapter = chapterList.First(x => x.CurrentChapterUrl == chapterUrl).NextChapterUrl;
            }
            if (!String.IsNullOrEmpty(nextChapter))
            {
                this.init(nextChapter);
            }
        }

        //private void GoToBeginning_Tapped(object sender, EventArgs e)
        //{
        //    this.AnimateButton(GoToBeginningImage);
        //}

        private void ShowSettings_Tapped(object sender, EventArgs e)
        {
            this.AnimateButton(ShowSettingsImage);
        }

        private async void AnimateButton(Image button)
        {
            const int _animationTime = 50;

            await button.FadeTo(0.5, _animationTime);
            await button.FadeTo(1, _animationTime);
        }

        #region Populate popup

        private void ShowChapterList_Tapped(object sender, EventArgs e)
        {
            this.AnimateButton(ShowChapterListImage);
            overlay.IsVisible = true;
            PageContentGrid.IsEnabled = false;
        }
        private void ClosePopUp_Clicked(object sender, EventArgs e)
        {
            overlay.IsVisible = false;
            PageContentGrid.IsEnabled = true;
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
                viewCell.View.BackgroundColor = (Color)Application.Current.Resources["BackgroundColor"];
                lastCell = viewCell;
            }
        }

        private void ChapterList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Model.Chapter.ItemModel ChosenChapter = (Model.Chapter.ItemModel)e.Item;

            this.init(ChosenChapter.CurrentChapterUrl);
        }

        //private async void InitializeChapterList(List<ItemModel> chapterList)
        //{
        //    await Task.Run(() =>
        //    {
        //        var CardWidth = Application.Current.MainPage.Width * 0.30;
        //        var CardHeight = Application.Current.MainPage.Height * 0.10;
        //        overlay.WidthRequest = Application.Current.MainPage.Width * 0.30;
        //        overlay.HeightRequest = Application.Current.MainPage.Height * 0.70;
        //        foreach (ItemModel chapter in chapterList)
        //        {
        //            Grid Card = new Grid
        //            {
        //                HorizontalOptions = LayoutOptions.StartAndExpand,
        //                VerticalOptions = LayoutOptions.CenterAndExpand,
        //                BackgroundColor = Color.Transparent,
        //                Margin = new Thickness(5, 5, 5, 0),
        //                HeightRequest = CardHeight,
        //                WidthRequest = CardWidth,
        //                RowDefinitions =
        //                {
        //                    new RowDefinition { Height = new GridLength(2) },
        //                    new RowDefinition(),
        //                },
        //            };
        //            BoxView spacer = new BoxView
        //            {
        //                Color = Color.Blue,
        //                HorizontalOptions = LayoutOptions.Fill,
        //            };
        //            Card.Children.Add(spacer, 0, 0);
        //            var tapGestureRecognizer = new TapGestureRecognizer
        //            {
        //                CommandParameter = chapter.CurrentChapterUrl,
        //            };
        //            tapGestureRecognizer.Tapped += this.SelectedChapter;
        //            Card.GestureRecognizers.Add(tapGestureRecognizer);
        //            Label name = new Label
        //            {
        //                Text = chapter.Title,
        //                FontSize = 12,
        //                TextColor = chapter.TitleColor,
        //                MaxLines = 2,
        //                HorizontalOptions = LayoutOptions.CenterAndExpand,
        //                VerticalOptions = LayoutOptions.CenterAndExpand,
        //                Margin = new Thickness(5, 0, 5, 5),
        //            };
        //            Card.Children.Add(name, 0, 1);
        //            Device.BeginInvokeOnMainThread(() =>
        //            {
        //                ChapterListLayout.Children.Add(Card);
        //            });
        //        }
        //    });
        //}
        //private void SelectedChapter(object sender, System.EventArgs e)
        //{
        //    string url = ((TappedEventArgs)e).Parameter.ToString();

        //    this.init(url);
        //}
        #endregion
    }
}