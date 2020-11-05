using HtmlAgilityPack;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;
using System.Web;
using WebScraperApp.Model.Chapter;

namespace WebScraperApp.Scrapers.NovelFull.Chapter
{
    public class ContentScraper
    {
        private ViewModel chapter = new ViewModel();
        public ViewModel Chapter
        {
            get { return chapter; }
            set { chapter = value; }
        }

        public void ScrapeChapterData(ParameterModel ParamModel)
        {
            string url = ParamModel.ChapterUrl;
            var web = new HtmlWeb();
            var doc = web.Load(url);

            // For the next chapter navigation
            chapter.CurrentUrl = ParamModel.ChapterUrl;
            var chapterData = doc.DocumentNode.SelectSingleNode("//*[@class = 'row']/div[@class = 'col-xs-12']");
            chapter.NovelName = HttpUtility.HtmlDecode(chapterData.SelectSingleNode(".//a[@class = 'truyen-title']").InnerText) ?? "";
            chapter.Name = HttpUtility.HtmlDecode(chapterData.SelectSingleNode(".//a[@class = 'chapter-title']").InnerText) ?? "";
            var contentContainer = doc.DocumentNode.SelectSingleNode("//*[@id = 'wrapper']/main[@id = 'container']/div[@class = 'chapter container']/div[@class = 'row']/div[@class = 'col-xs-12']/div[@class = 'chapter-c']");
            var chapterContent = contentContainer.SelectNodes(".//p");

            // Add new line between paragraphs
            foreach (var content in chapterContent)
            {
                if (!String.IsNullOrWhiteSpace(content.InnerText) && !content.InnerText.Contains("\n"))
                {
                    chapter.Content += " \n" + IndentedTextWriter.DefaultTabString + content.InnerText;
                }
            }
            // Get the quotations
            chapter.Content = this.TransformText(chapter.Content);
        }

        public string TransformText(string value)
        {
            string DecimalRightSingleQuotationMark = "&#8217";
            string TransformedRightSingleQuotationMark = "’";
            value = value.Replace(DecimalRightSingleQuotationMark, TransformedRightSingleQuotationMark);

            string DecimalLeftSingleQuotationMark = "&#8216";
            string TransformedLeftSingleQuotationMark = "‘";
            value = value.Replace(DecimalLeftSingleQuotationMark, TransformedLeftSingleQuotationMark);

            string DecimalQuotationMark = "&#8218";
            string TransformedQuotationMark = "\"";
            value = value.Replace(DecimalQuotationMark, TransformedQuotationMark);

            string DecimalSingle9QuotationMark = "&#8219";
            string TransformedSingle9QuotationMark = "‛";
            value = value.Replace(DecimalSingle9QuotationMark, TransformedSingle9QuotationMark);

            string DecimalLeftDoubleQuotationMark = "&#8220";
            string TransformedLeftDoubleQuotationMark = "“";
            value = value.Replace(DecimalLeftDoubleQuotationMark, TransformedLeftDoubleQuotationMark);

            string DecimalRightDoubleQuotationMark = "&#8221";
            string TransformedRightDoubleQuotationMark = "”";
            value = value.Replace(DecimalRightDoubleQuotationMark, TransformedRightDoubleQuotationMark);

            string Annoying = ";";
            string Empty = " ";
            value = value.Replace(Annoying, Empty);
            return value;
        }
    }
}
