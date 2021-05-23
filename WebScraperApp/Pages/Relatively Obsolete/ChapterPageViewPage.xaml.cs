using System;
using System.Collections.Generic;
using System.Linq;
using WebScraperApp.Model.Chapter;
using WebScraperApp.Scrapers.NovelFull;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WebScraperApp.Pages.Chapter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChapterPageViewPage : ContentPage
    {
        ChapterScrapper scraper;
        // Indext of the listview
        int currentPage;
        // not index so <= is needed instead of <
        int totalPages;
        string chapterUrl;
        // ChapterList for the navigation
        List<ItemModel> chapterList;

        public ChapterPageViewPage(string url, List<ItemModel> chapters)
        {
            InitializeComponent();

            // initialize the scrapter
            chapterList = chapters;
            this.init(url);
        }

        private void init(string url)
        {
            scraper = new ChapterScrapper();
            BindingContext = scraper;

            // Get parameters 
            ParameterModel paramModel = new ParameterModel();
            // Get the width and height for Page line Calculation
            if (paramModel.PageWidth == 0) paramModel.PageWidth = Application.Current.MainPage.Width;// - padding
            if (paramModel.PageHeight == 0) paramModel.PageHeight = Application.Current.MainPage.Height - ((CurrentPageLabel.FontSize * 4) + (ChapterName.FontSize * 6));
            if (paramModel.Density == 0) paramModel.Density = DeviceDisplay.MainDisplayInfo.Density;
            if (paramModel.FontSize == null) paramModel.FontSize = ChapterName.FontSize;
            chapterUrl = url;
            paramModel.ChapterUrl = chapterUrl;
            currentPage = 0;

            // Get data
            scraper.ScrapeChapterData(paramModel);
            // bind the data
            ViewModel chapter = scraper.Chapter;

            ChapterName.Text = chapter.Name;
            CurrentPageLabel.Text = 0.ToString();
            //TotalPagesLabel.Text = chapter.Pages.Count.ToString();
            //totalPages = chapter.Pages.Count;

            //Pages.ItemsSource = chapter.Pages;
            Pages.ScrollTo(0, animate: false);
            CurrentPageLabel.Text = "1";

        }

        private void Pages_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            //ChapterPageModel x = (ChapterPageModel)e.CurrentItem;
            //CurrentPageLabel.Text = x.CurrentPage.ToString();
        }

        private void PreviousButton_Clicked(object sender, EventArgs e)
        {
            if (currentPage + 1 > 1)
            {
                currentPage -= 1;
                Pages.ScrollTo(currentPage);
                CurrentPageLabel.Text = (currentPage + 1).ToString();
            }
            else if (currentPage + 1 == 1)
            {
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
        }

        private void ActivateLayout_Clicked(object sender, EventArgs e)
        {

        }
        private void NextButton_Clicked(object sender, EventArgs e)
        {
            if (currentPage + 1 < totalPages)
            {
                currentPage += 1;
                Pages.ScrollTo(currentPage);
                CurrentPageLabel.Text = (currentPage + 1).ToString();
            }
            else if (currentPage + 1 == totalPages)
            {
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
        }
    }
}