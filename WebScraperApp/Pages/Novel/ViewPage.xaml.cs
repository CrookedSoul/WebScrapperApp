using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WebScraperApp.Pages.Novel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewPage : TabbedPage
    {
        public string NovelUrl { get; set; }
        public ViewPage(string novelUrl)
        {
            InitializeComponent();

            this.AddDescriptionPage(novelUrl);
            this.AddChapterList(novelUrl);
            //NavigationPage.BarBackgroundColorProperty 
            this.BackgroundColor = (Color)Application.Current.Resources["BackgroundColor"];
            this.UnselectedTabColor = (Color)Application.Current.Resources["UnselectedTabColor"];
            this.SelectedTabColor = (Color)Application.Current.Resources["SelectedTabColor"];
            this.BarBackgroundColor = (Color)Application.Current.Resources["BackgroundColor"];

        }
        public void AddChapterList(string novelUrl)
        {
            Pages.Chapter.ListPage page = new Pages.Chapter.ListPage(novelUrl);
            //page.IconImageSource = "icon.png";
            page.Title = "Chapters";

            this.Children.Add(page);
        }
        public void AddDescriptionPage(string novelUrl)
        {
            DescriptionPage page = new DescriptionPage(novelUrl);
            //page.IconImageSource = "descriptionIcon.png";//ImageSource.FromUri(new Uri("https://winaero.com/blog/wp-content/uploads/2019/11/Photos-new-icon.png"));
            page.Title = "Description";
            this.Children.Add(page);
        }
    }
}