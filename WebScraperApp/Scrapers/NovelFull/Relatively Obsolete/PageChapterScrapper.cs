using HtmlAgilityPack;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebScraperApp.Model.Chapter;

namespace WebScraperApp.Scrapers.NovelFull
{
    public class ChapterScrapper
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
            chapter.Pages = new List<PageModel>();
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

            string[] words = chapter.Content.Split(' ');
            var lineLength = (ParamModel.PageWidth / ParamModel.FontSize) * 2;
            var numberOfLines = ParamModel.PageHeight / ParamModel.FontSize;

            int nextWord = 0;
            string CurrentLine = "";

            // Create the lines
            List<string> Lines = new List<string>();
            foreach (var word in words)
            {
                nextWord++;
                string currentWord = " " + word;
                if (currentWord.Length == 0) currentWord = " ";
                if ((currentWord.Length + CurrentLine.Length) < lineLength)
                {
                    if (String.IsNullOrEmpty(CurrentLine))
                    {
                        CurrentLine = currentWord;
                    }
                    else
                    {
                        CurrentLine += currentWord;
                    }
                    // check if we should add the page by checking if it contains new line breaker
                    if (CurrentLine.Contains(" \n"))
                    {
                        if (CurrentLine != "   \n" && CurrentLine != "  \n" && CurrentLine != " \n" && CurrentLine != "\n" && CurrentLine != " \n \n")
                        {
                            Lines.Add(CurrentLine);
                        }
                        CurrentLine = "";
                    }
                }
                // Add the line cause its too big
                else
                    // ovde skipa neke reci 
                    //if ((CurrentLine.Length >= lineLength) ||
                    //   ((nextWord >= 0 && nextWord < words.Length) && (words[nextWord].Length + CurrentLine.Length) >= lineLength))
                {
                    if (!CurrentLine.Contains("\n"))
                    {
                        CurrentLine += " \n";
                    }
                    Lines.Add(CurrentLine);
                    CurrentLine = currentWord;
                }
            }
            if (!String.IsNullOrEmpty(CurrentLine))
            {
                Lines.Add(CurrentLine);
            }

            // Add lines into pages
            int currentPage = 1;
            int totalLineCount = 1;
            List<string> PageLines = new List<string>();
            int maximumLinesInAPage = (int)(Math.Floor(numberOfLines.Value));
            PageModel page = new PageModel();
            foreach (string line in Lines)
            {
                if ((PageLines.Count < maximumLinesInAPage) && (totalLineCount < Lines.Count))
                {
                    PageLines.Add(line);
                    if (page.CurrentPage == null) page.CurrentPage = currentPage;
                }
                // check if we should add the page by checking the lines
                else //if ((PageLines.Count >= maximumLinesInAPage) || (totalLineCount >= Lines.Count))
                {
                    page.Content = "";
                    foreach (string pageline in PageLines)
                    {
                        page.Content += pageline;
                    }
                    chapter.Pages.Add(page);
                    page = new PageModel();
                    currentPage++;
                    if (page.CurrentPage == null) page.CurrentPage = currentPage;
                    PageLines = new List<string>();
                    PageLines.Add(line);
                }
                totalLineCount++;
            }
            chapter.TotalPages = chapter.Pages[chapter.Pages.Count - 1].CurrentPage.Value;
        }

        static IEnumerable<string> GetLines(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize).TrimEnd());
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
        #region old useless code
        //int onePage = (int)Math.Round(lineLength.Value * numberOfLines.Value);
        //        // Should make it so that first it gets every line and THEN adds llines that can show on the screen
        //        // Add words into pages
        //        foreach (var word in words)
        //        {
        //            nextWord++;
        //            string currentWord = word;
        //            if (currentWord.Length == 0) currentWord = " ";
        //            if ((currentWord.Length + charCount) < lineLength* numberOfLines)
        //            {
        //                if (page.CurrentPage == null) page.CurrentPage = i;
        //                //if (page.TotalPages == null) page.TotalPages = chapter.TotalPages;
        //                if (String.IsNullOrEmpty(page.Content))
        //                {
        //                    page.Content = " " + currentWord;
        //                }
        //                else
        //                {
        //                    page.Content += " " + currentWord;
        //                }
        //                charCount += currentWord.Length;
        //                // check if we should add the page by checking if its not last word and if its next added word will make the chunk bigger than the defined page 
        //                if ((charCount >= onePage) || ((nextWord >= 0 && nextWord<words.Length) && (words[nextWord].Length + charCount) >= onePage))
        //                {
        //                    chapter.Pages.Add(page);
        //                    page = new PageModel();
        //                    charCount = 0;
        //                    i++;
        //                }
        //            }
        //            // check if the page was added
        //            else if ((currentWord.Length + charCount) > onePage)
        //            {
        //                page = new PageModel();
        //                charCount = 0;
        //                i++;
        //            }
        //        }
        //        if (!chapter.Pages.Exists(x => x.CurrentPage == page.CurrentPage))
        //        {
        //            chapter.Pages.Add(page);
        //            i++;
        //        }
        #endregion
    }
}
