using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraperApp.Model.Chapter;
using WebScraperApp.Scrapers.NovelFull;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WebScraperApp.Scrapers.NovelFull.Chapter;

namespace WebScraperApp.Pages.Novel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChapterListPage : ContentPage
    {
        ViewCell lastCell;
        ListScraper chapterList;

        public ChapterListPage(string novelUrl)
        {
            InitializeComponent();

            chapterList = new ListScraper();

            // The colors
            chapterList.BackgroundColor = Color.FromHex("0e1821");
            chapterList.SeparatorColor = Color.FromHex("62c6f5");
            chapterList.TitleColor = Color.FromHex("d9ebe9");
            chapterList.DownloadedColor = Color.Transparent;//Color.FromHex("62c6f5");

            chapterList.StartScraping(novelUrl);
            BindingContext = chapterList;
            // bind the view model's List property to the list view's ItemsSource:
            ChapterList.SetBinding(ListView.ItemsSourceProperty, "List");
        }

        private void ChapterList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Model.Chapter.ItemModel ChosenChapter = (Model.Chapter.ItemModel)e.SelectedItem;

            Navigation.PushAsync(new ChapterPage(ChosenChapter.CurrentChapterUrl, chapterList.List.ToList()));
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            if (lastCell != null)
            {
                lastCell.View.BackgroundColor = Color.FromHex("0e1821");
            }
            var viewCell = (ViewCell)sender;
            //if (lastCell != viewCell)
            //{
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.FromHex("0e1821");
                lastCell = viewCell;
            }
            //}
        }

        private void ChapterList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Model.Chapter.ItemModel ChosenChapter = (Model.Chapter.ItemModel)e.Item;

            Navigation.PushAsync(new ChapterPage(ChosenChapter.CurrentChapterUrl, chapterList.List.ToList()));
        }
    }
}