using HtmlAgilityPack;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Web;
using WebScraperApp.Model.Chapter;
using Xamarin.Forms;

namespace WebScraperApp.Scrapers.NovelFull.Chapter
{
    public class ListScraper : INotifyPropertyChanged
    {
        // Settings
        public Color BackgroundColor { get; set; }
        public Color SeparatorColor { get; set; }
        public Color DownloadedColor { get; set; }
        public Color TitleColor { get; set; }

        // property changed event handler
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<ItemModel> _list;

        public ObservableCollection<ItemModel> List
        {
            get { return _list; }
            set
            {
                _list = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(List)));
            }
        }

        public ListScraper()
        {
            _list = new ObservableCollection<ItemModel>();
        }

        public async void StartScraping(string novelUrl)
        {
            var web = new HtmlWeb();
            var doc = web.Load(novelUrl);

            var FirstPage = doc.DocumentNode.SelectSingleNode("//*[@id = 'list-chapter']/ul[@class = 'pagination pagination-sm']/li[@class = 'active']/a");
            var LastPage = doc.DocumentNode.SelectSingleNode("//*[@id = 'list-chapter']/ul[@class = 'pagination pagination-sm']/li[@class = 'last']/a");
            string numberOfPages = HttpUtility.HtmlDecode(LastPage.GetAttributeValue("data-page", "1"));
            // assuming you are 'awaiting' the results of your scraping method...
            for (int i = 1; i <= (int.Parse(numberOfPages) + 1); i++)
            {
                await Task.Run(() =>
                {
                    var pageUrl = "https://novelfull.com" + FirstPage.GetAttributeValue("href", "empty").Replace("?page=1", "?page=" + i.ToString());
                    // Get pageURL and replace the page with the i
                    if (pageUrl == "empty" || pageUrl.Contains("void") || String.IsNullOrEmpty(pageUrl))
                    {
                        return;
                    }

                    // If theres no page in the url add it (they might remove it so who knows)
                    if (!pageUrl.Contains("?page"))
                    {
                        pageUrl += $"?page={i}";
                    }

                    doc = web.Load(pageUrl);

                    var Chapters = doc.DocumentNode.SelectNodes("//*[@id = 'list-chapter']/div[@class = 'row']/div[@class = 'col-xs-12 col-sm-6 col-md-6']/ul/li");
                    foreach (var chapter in Chapters)
                    {
                        ItemModel model = new ItemModel();
                        model.Title = HttpUtility.HtmlDecode(chapter.SelectSingleNode(".//a").InnerText) ?? "";
                        if (chapter.PreviousSibling != null)
                        {
                            model.PreviousChapterUrl = "https://novelfull.com" + (HttpUtility.HtmlDecode(chapter.PreviousSibling.SelectSingleNode(".//a").GetAttributeValue("href", "")) ?? "");
                        }
                        model.CurrentChapterUrl = "https://novelfull.com" + (HttpUtility.HtmlDecode(chapter.SelectSingleNode(".//a").GetAttributeValue("href", "")) ?? "");
                        if (chapter.NextSibling != null)
                        {
                            model.NextChapterUrl = "https://novelfull.com" + (HttpUtility.HtmlDecode(chapter.NextSibling.SelectSingleNode(".//a").GetAttributeValue("href", "")) ?? "");
                        }

                        model.TitleColor = TitleColor;
                        model.DownloadedColor = DownloadedColor;

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            List.Add(model);
                        });
                        //_list.Add(model);
                    }

                    // if you instead have multiple items to add at this point,
                    // then just create a new List<Mainlist>, add your items to it,
                    // then add that list to the ObservableCollection List.
                });
            }

        }
    }
}
