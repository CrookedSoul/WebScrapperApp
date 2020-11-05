using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace WebScraperApp.Model.Novel
{
    public class ViewModel
    {
        public string ImageURL { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Source { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public List<Chapter.ItemModel> ChapterList { get; set; }
        public List<TagModel> TagList { get; set; }

        // Settings

        public Color BackgroundColor { get; set; }
        public Color TextColor { get; set; }
        public Color TagTextColor { get; set; }
    }
}
