using System;
using System.Collections.Generic;
using System.Text;

namespace WebScraperApp.Model.SiteConst
{
    public class ReadWebNovelGlobal
    {
        // Maybe i can make constants for the Html paths and just switch them depending on the site but not for now
        public const string CatalogOpenUrl = "https://readwebnovels.net/page/";
        public const string SearchByGenreUrl = "https://readwebnovels.net/novel-genre/#GENRE#/page/#PAGE#/";
        public const string SearchByNameUrl = "https://readwebnovels.net/page/#PAGE#/?s=#NAME#&post_type=wp-manga&op=&author=&artist=&release=&adult="; // min 3 letters
        public const string NamePage = "&page=";
        public const string TagPage = "?page=";
        public const int CatalogOpen = 1;
        public const int SearchByName = 2;
        public const int SearchByTag = 3;

        public string GetSearchUrl(SearchModel model)
        {
            string result = SearchByNameUrl.Replace("#PAGE#", model.Page);
            result = SearchByNameUrl.Replace("#NAME#", model.Name);
            return result;
        }

        public string GetCatalogUrl(SearchModel model)
        {
            string result = CatalogOpenUrl + (model.Page ?? "1");
            return result;
        }

        public string GetGenreUrl(SearchModel model)
        {
            string result = SearchByGenreUrl.Replace("#PAGE#", model.Page);
            result = SearchByNameUrl.Replace("#GENRE#", model.Genre);
            return result;
        }

        #region

        //switch (searchType)
        //{
        //    case NovelFullConst.CatalogOpen:
        //        url = NovelFullConst.CatalogOpenUrl + page;
        //        break;
        //    case NovelFullConst.SearchByName:
        //        url = NovelFullConst.SearchByNameUrl + value;
        //        break;
        //    case NovelFullConst.SearchByTag:
        //        url = NovelFullConst.SearchByTagUrl + value;
        //        break;
        //    default:
        //        url = NovelFullConst.CatalogOpenUrl;
        //        break;
        //}
        #endregion
    }
}
