using System;
using System.Collections.Generic;
using System.Text;

namespace WebScraperApp.Model.SiteFunctions
{
    public class NovelFullFunction
    {
        // Maybe i can make constants for the Html paths and just switch them depending on the site but not for now
        public const string CatalogOpenUrl = "https://novelfull.com/index.php/latest-release-novel?page=";
        public const string SearchByGenreUrl = "https://novelfull.com/index.php/genre/#GENRE#?page=#PAGE#";
        public const string SearchByNameUrl = "https://novelfull.com/index.php/search?keyword=#NAME#&page=#PAGE#";


        public string GetSearchUrl(SearchModel model)
        {
            string result = SearchByNameUrl.Replace("#PAGE#", model.Page);
            result = result.Replace("#NAME#", model.Name);
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
            result = result.Replace("#GENRE#", model.Genre);
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
