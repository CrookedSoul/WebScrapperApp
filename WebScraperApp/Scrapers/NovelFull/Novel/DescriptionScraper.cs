using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Web;
using WebScraperApp.Model.Novel;

namespace WebScraperApp.Scrapers.NovelFull.Novel
{
    public class DescriptionScraper
    {
        private ViewModel novel = new ViewModel();
        public ViewModel Novel
        {
            get { return novel; }
            set { novel = value; }
        }

        public void ScrapeNovelData(string novelUrl)
        {
            string url = novelUrl;
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var book = doc.DocumentNode.SelectSingleNode("//*[@class = 'books']");
            novel.ImageURL = "https://novelfull.com" + (HttpUtility.HtmlDecode(book.SelectSingleNode(".//div[@class = 'book']/img").GetAttributeValue("src", "")));
            novel.Title = /*"Title: " +*/ doc.DocumentNode.SelectSingleNode("//*[@class = 'col-xs-12 col-sm-8 col-md-8 desc']/h3[@class = 'title']").InnerText ?? "";
            novel.Description = doc.DocumentNode.SelectSingleNode("//*[@class = 'col-xs-12 col-sm-8 col-md-8 desc']/div[@class = 'desc-text']").InnerText ?? "";

            //info div
            var info = doc.DocumentNode.SelectSingleNode("//*[@class = 'info']");
            // Contains AuthorNames Genres Status and Source
            var infoDivs = info.SelectNodes(".//div");

            //var authorNames = infoDivs[0].SelectNodes(".//a");
            // Theres an a href for it if i want to link it to search function
            novel.Author = /*"Author: " +*/ this.GetInfo("Author", infoDivs);//this.GetNodeInnerText(infoDivs[0]);
            novel.Source = /*"Source: " +*/ this.GetInfo("Source", infoDivs);//this.GetNodeInnerText(infoDivs[3]);
            novel.Status = /*"Status: " +*/ this.GetInfo("Status", infoDivs);//this.GetNodeInnerText(infoDivs[4]);

            // Napravi dinamicki kao GetInfo sto ima;
            var tagList = infoDivs[2].SelectNodes(".//a");
            novel.TagList = this.GetTagList(tagList);

            novel.ChapterList = new List<Model.Chapter.ItemModel>();
        }

        private List<TagModel> GetTagList(HtmlNodeCollection tagList)
        {
            List<TagModel> TagList = new List<TagModel>();
            foreach (HtmlNode tag in tagList)
            {
                TagModel model = new TagModel();
                model.Name = tag.InnerText;
                model.Url = tag.GetAttributeValue("href", "");
                if (!String.IsNullOrEmpty(model.Url))
                {
                    TagList.Add(model);
                }
            }
            return TagList;
        }

        private string GetNodeInnerText(HtmlNode node)
        {
            HtmlNodeCollection nodeCollection = node.SelectNodes(".//a");
            string result = "";
            if (nodeCollection != null)
            {
                foreach (HtmlNode name in nodeCollection)
                {
                    result += " " + name.InnerText ?? "";
                }
            }
            if (String.IsNullOrEmpty(result) && !String.IsNullOrEmpty(node.InnerText ?? ""))
            {
                result = node.InnerText;
                result = result.Substring(result.IndexOf("Source:") + ("Source:").Length);
            }
            return result;
        }

        private string GetNodeInnerInfo(string InfoParameter, HtmlNode node)
        {
            HtmlNode h3Node = node.SelectSingleNode(".//h3");
            if (h3Node.InnerText.ToLower().Contains(InfoParameter.ToLower()))
            {
                string result = this.GetNodeInnerText(node);
                if (string.IsNullOrEmpty(result) && InfoParameter.ToLower() == "Source".ToLower()) node.InnerHtml.Substring(node.InnerHtml.IndexOf("</h3>") + node.InnerHtml.Length);
                return result;
            }
            else
            {
                return "";
            }
        }
        private string GetInfo(string InfoParameter, HtmlNodeCollection nodes)
        {
            foreach (HtmlNode node in nodes)
            {
                string result = this.GetNodeInnerInfo(InfoParameter, node);
                if (!string.IsNullOrEmpty(result)) return result;
            }

            return "NOT FOUND BITCHES";
        }
    }
}
