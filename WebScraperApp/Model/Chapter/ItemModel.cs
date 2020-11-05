using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace WebScraperApp.Model.Chapter
{
    public class ItemModel
    {
        public string Title { get; set; }
        public string CurrentChapterUrl { get; set; }
        public string PreviousChapterUrl { get; set; }
        public string NextChapterUrl { get; set; }

        // Settings
        public Color TitleColor { get; set; }
        public Color DownloadedColor { get; set; }
    }
}
