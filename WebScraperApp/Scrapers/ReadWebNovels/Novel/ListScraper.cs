using HtmlAgilityPack;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebScraperApp.Model;
using WebScraperApp.Model.Novel;
using WebScraperApp.Model.SiteConst;
using Xamarin.Forms;

namespace WebScraperApp.Scrapers.ReadWebNovels.Novel
{
    public class ListScraper
    {
        // Was observableCollections
        private List<ItemModel> list = new List<ItemModel>();
        private ReadWebNovelGlobal global = new ReadWebNovelGlobal();

        public Color BackgroundColor { get; set; }
        public Color BorderColor { get; set; }
        public Color TextColor { get; set; }

        public List<ItemModel> List
        {
            get { return list; }
            set { list = value; }
        }

        public /*async*/ void ScrapeNovelListData(string page, string value, int searchType)
        {
            if (string.IsNullOrEmpty(page)) page = "1";

            //await Task.Run(() =>
            //{
            string url = "";
            SearchModel searchModel = new SearchModel();
            searchModel.Page = page;
            if (searchType == ReadWebNovelGlobal.CatalogOpen)
            {
                url = global.GetCatalogUrl(searchModel);
            }
            else if (searchType == ReadWebNovelGlobal.SearchByName && !string.IsNullOrEmpty(value))
            {
                searchModel.Name = value;
                url = global.GetSearchUrl(searchModel);
            }
            else if (searchType == ReadWebNovelGlobal.SearchByTag && !string.IsNullOrEmpty(value))
            {
                searchModel.Genre = value;
                url = global.GetGenreUrl(searchModel);
            }

            var web = new HtmlWeb();
            var doc = web.Load(url);

            var Novels = doc.DocumentNode.SelectNodes("//*[@class = 'list list-truyen col-xs-12']/div[@class = 'row']");

            if (Novels != null)
            {
                foreach (var novel in Novels)
                {
                    var imageUrl = "https://novelfull.com" + HttpUtility.HtmlDecode(novel.SelectSingleNode(".//div[@class = 'col-xs-3']/div/img[@class = 'cover']").GetAttributeValue("src", "")) ?? "";
                    var title = HttpUtility.HtmlDecode(novel.SelectSingleNode(".//div[@class = 'col-xs-7']/div/h3[@class = 'truyen-title']").InnerText) ?? "";
                    var author = HttpUtility.HtmlDecode(novel.SelectSingleNode(".//div[@class = 'col-xs-7']/div/span[@class = 'author']").InnerText) ?? "";
                    var novelUrl = "https://novelfull.com" + HttpUtility.HtmlDecode(novel.SelectSingleNode(".//div[@class = 'col-xs-7']/div/h3[@class = 'truyen-title']/a").GetAttributeValue("href", "")) ?? "";
                    //var latestChapter = (HttpUtility.HtmlDecode(novel.SelectSingleNode(".//div[@class = 'col-xs-2 text-info']/div/a/span[@class = 'chapter-text']").InnerText) ?? "").Substring(("Chapter -").Count());


                    ItemModel model = new ItemModel();
                    model.ImageURL = imageUrl;
                    model.Title = title;
                    model.Author = author;
                    model.NovelUrl = novelUrl;
                    //model.LatestChapter = latestChapter;

                    model.BackgroundColor = BackgroundColor;
                    model.BorderColor = BorderColor;
                    model.TextColor = TextColor;
                    //Device.BeginInvokeOnMainThread(() =>
                    //{
                    list.Add(model);
                    //});
                }
            }
            else
            {
                ClearCache();
            }
            //});
        }

        public void ClearCache()
        {
            List = new List<ItemModel>();
        }
    }
}
