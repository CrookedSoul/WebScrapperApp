using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace WebScraperApp.Model.Novel
{
    public class ItemModel
    {
        // moram staviti List<ItemModel>
        public string ImageURL { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string NovelUrl { get; set; }
        public string LatestChapter { get; set; }

        public Color BackgroundColor { get; set; }
        public Color BorderColor { get; set; }
        public Color TextColor { get; set; }
    }
}
