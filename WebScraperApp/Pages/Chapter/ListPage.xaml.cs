using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraperApp.Scrapers.NovelFull.Chapter;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WebScraperApp.Pages.Chapter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPage : ContentPage
    {
        ViewCell lastCell;
        ListScraper chapterList;

        public ListPage(string novelUrl)
        {
            InitializeComponent();

            chapterList = new ListScraper();

            // The colors
            chapterList.BackgroundColor = (Color)Application.Current.Resources["BackgroundColor"];
            chapterList.SeparatorColor = (Color)Application.Current.Resources["SeparatorColor"];
            chapterList.TitleColor = (Color)Application.Current.Resources["TitleColor"];
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
                lastCell.View.BackgroundColor = (Color)Application.Current.Resources["BackgroundColor"];
            }
            var viewCell = (ViewCell)sender;
            //if (lastCell != viewCell)
            //{
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = (Color)Application.Current.Resources["BackgroundColor"];
                lastCell = viewCell;
            }
            //}
        }

        private void ChapterList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Model.Chapter.ItemModel ChosenChapter = (Model.Chapter.ItemModel)e.Item;

            Navigation.PushAsync(new ReaderPage(ChosenChapter.CurrentChapterUrl, chapterList.List.ToList()));
        }
    }
}