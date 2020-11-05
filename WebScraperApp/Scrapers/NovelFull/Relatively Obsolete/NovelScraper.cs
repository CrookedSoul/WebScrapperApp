using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Web;
using WebScraperApp.Model.Novel;

namespace WebScraperApp.Scrapers.NovelFull
{
    public class NovelScraper
    {
        private ViewModel novel = new ViewModel();
        public ViewModel Novel 
        {
            get { return novel; }
            set { novel  = value; }
        }

        public void ScrapeNovelData(string novelPage)
        {
            string url = novelPage;
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var book = doc.DocumentNode.SelectSingleNode("//*[@class = 'books']");
            novel.ImageURL = "https://novelfull.com" + (HttpUtility.HtmlDecode(book.SelectSingleNode(".//div[@class = 'book']/img").GetAttributeValue("src", "")));
            novel.Title = doc.DocumentNode.SelectSingleNode("//*[@class = 'col-xs-12 col-sm-8 col-md-8 desc']/h3[@class = 'title']").InnerText ?? "";
            novel.Description = doc.DocumentNode.SelectSingleNode("//*[@class = 'col-xs-12 col-sm-8 col-md-8 desc']/div[@class = 'desc-text']").InnerText ?? "";

            novel.ChapterList = new List<Model.Chapter.ItemModel>();

            // Get all the pagination and shuffle through all of them
            var FirstPage = doc.DocumentNode.SelectSingleNode("//*[@id = 'list-chapter']/ul[@class = 'pagination pagination-sm']/li[@class = 'active']/a");
            var LastPage = doc.DocumentNode.SelectSingleNode("//*[@id = 'list-chapter']/ul[@class = 'pagination pagination-sm']/li[@class = 'last']/a");
            string numberOfPages = (HttpUtility.HtmlDecode(LastPage.GetAttributeValue("data-page", "1")));
            for (int i = 1; i <= int.Parse(numberOfPages); i++)
            {
                // Get pageURL and replace the page with the i
                var pageUrl = "https://novelfull.com" + (FirstPage.GetAttributeValue("href", "empty")).Replace("?page=1", "?page=" + i.ToString()); ;
                if (pageUrl == "empty" || pageUrl.Contains("void") || String.IsNullOrEmpty(pageUrl))
                {
                    continue;
                }
                doc = web.Load(pageUrl);

                var Chapters = doc.DocumentNode.SelectNodes("//*[@id = 'list-chapter']/div[@class = 'row']/div[@class = 'col-xs-12 col-sm-6 col-md-6']/ul/li");
                foreach (var chapter in Chapters)
                {
                    Model.Chapter.ItemModel model = new Model.Chapter.ItemModel();
                    model.Title = HttpUtility.HtmlDecode(chapter.SelectSingleNode(".//a").InnerText) ?? "";
                    if(chapter.PreviousSibling != null)
                    {
                        model.PreviousChapterUrl = "https://novelfull.com" + (HttpUtility.HtmlDecode(chapter.PreviousSibling.SelectSingleNode(".//a").GetAttributeValue("href", "")) ?? "");
                    }
                    model.CurrentChapterUrl = "https://novelfull.com" + (HttpUtility.HtmlDecode(chapter.SelectSingleNode(".//a").GetAttributeValue("href", "")) ?? "");
                    if(chapter.NextSibling != null)
                    {
                        model.NextChapterUrl = "https://novelfull.com" + (HttpUtility.HtmlDecode(chapter.NextSibling.SelectSingleNode(".//a").GetAttributeValue("href", "")) ?? "");
                    }
                    novel.ChapterList.Add(model);
                }
            }
        }
    }
}
