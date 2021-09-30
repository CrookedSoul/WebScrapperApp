using System;
using System.Collections.Generic;
using System.Text;

namespace WebScraperApp.Model
{
    public enum MenuItemType
    {
        MyLibrary,
        Browse,
        UserProfile
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
