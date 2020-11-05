using HtmlAgilityPack;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using WebScraperApp.Model.Novel;
using Xamarin.Forms;

namespace WebScraperApp.Scrapers.NovelFull
{
    public class NovelListScraper
    {
        private List<ItemModel> novelList = new List<ItemModel>();

        public Color BackgroundColor { get; set; }
        public Color BorderColor { get; set; }
        public Color TextColor { get; set; }

        public List<ItemModel> NovelList 
        {
            get { return novelList; }
            set { novelList  = value; }
        }

        public void ScrapeNovelListData(string page)
        {
            string url = "https://novelfull.com/index.php/latest-release-novel?page=" + page;
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var Novels = doc.DocumentNode.SelectNodes("//*[@class = 'list list-truyen col-xs-12']/div[@class = 'row']");

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
                novelList.Add(model);
            }
        }
    }
}
