using WebScraperApp.Model.Novel;
using WebScraperApp.Scrapers.NovelFull;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WebScraperApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NovelPage : ContentPage
    {
        NovelScraper scraper;
        ViewModel novel;
        public NovelPage(string novelUrl)
        {
            InitializeComponent();
            // initialize the scrapter
            scraper = new NovelScraper();
            // Get data
            scraper.ScrapeNovelData(novelUrl);

            // bind the data
            novel = scraper.Novel;
            BindingContext = novel;
            ChapterList.ItemsSource = novel.ChapterList;
        }

        private void ChapterList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Model.Chapter.ItemModel ChosenChapter = (Model.Chapter.ItemModel)e.SelectedItem;

            Navigation.PushAsync(new ChapterPage(ChosenChapter.CurrentChapterUrl, novel.ChapterList));
        }
    }
}