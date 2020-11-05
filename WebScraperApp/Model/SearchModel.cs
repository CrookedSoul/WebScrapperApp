using System;
using System.Collections.Generic;
using System.Text;

namespace WebScraperApp.Model
{
    public class SearchModel
    {
        public const int CatalogOpen = 1;
        public const int SearchByName = 2;
        public const int SearchByGenre = 3;

        public string Page { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
    }
}
