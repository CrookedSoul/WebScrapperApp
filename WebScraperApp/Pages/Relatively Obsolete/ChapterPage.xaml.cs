using System;
using System.Collections.Generic;
using System.Linq;
using WebScraperApp.Model.Chapter;
using WebScraperApp.Scrapers.NovelFull;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace WebScraperApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChapterPage : ContentPage
    {
        ChapterScrapper scraper;
        // Indext of the listview
        int currentPage;
        // not index so <= is needed instead of <
        int totalPages;
        string chapterUrl;
        // ChapterList for the navigation
        List<ItemModel> chapterList;
        //ViewModel chapter;
        public ChapterPage(string url, List<ItemModel> chapters)
        {
            InitializeComponent();
            // initialize the scrapter
            chapterList = chapters;
            this.init(url);

            //ChapterPageModel test = new ChapterPageModel();
            //test.Content = lorem;
            //test.CurrentPage = 1;
            //test.TotalPages = 1;
            //List<ChapterPageModel> listTest = new List<ChapterPageModel>();
            //listTest.Add(test);

            //Pages.ItemsSource = listTest;

            //this.CalculateDP();
        }

        private void init(string url)
        {
            // Flush all the possibly remaining stuff first
            //Pages.ItemsSource = null;
            scraper = new ChapterScrapper();
            BindingContext = scraper;

            // Get parameters 
            ParameterModel paramModel = new ParameterModel();
            // Get the width and height for Page line Calculation
            if (paramModel.PageWidth == 0) paramModel.PageWidth = (int)Math.Round(Application.Current.MainPage.Width);
            if (paramModel.PageHeight == 0) paramModel.PageHeight = (int)Math.Round(Application.Current.MainPage.Height);
            chapterUrl = url;
            paramModel.ChapterUrl = chapterUrl;
            currentPage = 0;
            //StringReader reader = new StringReader(Pages.ToString());
            // Get data
            scraper.ScrapeChapterData(paramModel);
            // bind the data
            ViewModel chapter = scraper.Chapter;
            //if (chapter.ChapterList == null) chapter.ChapterList = chapterList;
            //NovelName.Text = chapter.NovelName;
            ChapterName.Text = chapter.Name;
            CurrentPageLabel.Text = 0.ToString();
            TotalPagesLabel.Text = chapter.Pages.Count.ToString();
            totalPages = chapter.Pages.Count;

            Pages.ItemsSource = chapter.Pages;
            Pages.ScrollTo(0, animate: false);
            CurrentPageLabel.Text = "1";

        }

        private void Pages_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            //ChapterPageModel x = (ChapterPageModel)e.CurrentItem;
            //CurrentPageLabel.Text = x.CurrentPage.ToString();
        }

        private void CalculateDP()
        {

            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            // Width (in pixels)
            var width = mainDisplayInfo.Width;
            // Width (in xamarin.forms units)
            var xamarinWidth = width / mainDisplayInfo.Density;
            // Height (in pixels)
            var height = mainDisplayInfo.Height;
            // Screen density
            var density = mainDisplayInfo.Density;

            var unitWidth = (double)((width - 0.5f) / density);
            var unitHeight = (double)((height - 0.5f) / density);

            //int screenWidth = (int)Math.Round(this.Width);
            //int screenHeight = (int)Math.Round(this.Height);

            //var px = Math.Sqrt((this.Width * this.Width) + (this.Height * this.Height));

            //int points = 72; // each point in pixels 
            //int fontSize = 16; // font size
            //int dpi = 96; // find screen dpi (96 is very common I know)
            //int px = fontSize * dpi / points; // each lines need px (exp: 16)
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
                // newUrl = ChangeChapter(chapterUrl, false);
                //this.init(newUrl);
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
                //string newUrl = ChangeChapter(chapterUrl, true);
                //this.init(newUrl);
            }
        }

        //private string ChangeChapter(string url, bool next)
        //{
        //    string result = null;
        //    int numberPosition = url.IndexOf("chapter-", 1) + "chapter-".Count();
        //    var changeChapter = new StringBuilder(url);
        //    int currentChapter = int.Parse(url[numberPosition].ToString());
        //    changeChapter.Remove(numberPosition, 1);
        //    if (next)
        //    {
        //        // need to put the check for the last chapter also
        //        changeChapter.Insert(numberPosition, (currentChapter + 1).ToString());
        //        result = changeChapter.ToString();
        //    }
        //    else
        //    {
        //        if (currentChapter > 1)
        //        {
        //            changeChapter.Insert(numberPosition, (currentChapter - 1).ToString());
        //            result = changeChapter.ToString();
        //        }
        //        else
        //        {
        //            return result;
        //        }
        //    }
        //    return result;
        //}
    }
}