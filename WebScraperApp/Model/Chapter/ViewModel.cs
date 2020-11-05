using System;
using System.Collections.Generic;
using System.Text;

namespace WebScraperApp.Model.Chapter
{
    public class ViewModel
    {
        public string NovelName { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string CurrentUrl { get; set; }
        public List<ItemModel> ChapterList { get; set; }

        #region depracated
        public List<PageModel> Pages { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        #endregion
    }
}
