using System.Threading.Tasks;
using WebScraperApp.Model.Novel;
using WebScraperApp.Model.SiteConst;
using WebScraperApp.Model.SiteFunctions;
using WebScraperApp.Scrapers.NovelFull.Novel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WebScraperApp.Pages.Novel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DescriptionPage : ContentPage
    {
        DescriptionScraper scraper;
        ViewModel model;
        string novelUrl;
        bool hasAppeared = false;
        protected override void OnAppearing()
        {
            if (!hasAppeared)
            {
                this.init();
                hasAppeared = true;
            }

            base.OnAppearing();
        }
        public DescriptionPage(string novelUrl)
        {
            InitializeComponent();

            this.novelUrl = novelUrl;
        }

        private async void init()
        {
            // initialize the scrapter
            scraper = new DescriptionScraper();

            await Task.Run(() =>
            {
                // Get data
                scraper.ScrapeNovelData(novelUrl);

                // get the scraped data
                model = scraper.Novel;

                // The colors
                model.BackgroundColor = Color.FromHex("0e1821");
                model.TextColor = Color.FromHex("fffafa");
                model.TagTextColor = Color.FromHex("62c6f5");

                //Device.BeginInvokeOnMainThread(() =>
                //{
                foreach (TagModel tag in model.TagList)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        this.AddTags(tag.Name);
                    });
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    // Bind the data
                    BindingContext = model;
                });
                //});

            });

            //NovelGrid.BackgroundColor = Color.FromHex("0e1821");
        }

        private void AddTags(string Name)
        {
            Button button = new Button
            {
                Text = Name,
                Margin = new Thickness(0, 0, 5, 5),
                FontSize = 12,
                Padding = new Thickness(10,0,10,0),
                TextColor = model.TagTextColor,
                BackgroundColor = Color.Transparent,
                BorderColor = model.TagTextColor,
                CornerRadius = 15,
                BorderWidth = 2,
                HeightRequest = 25,
                HorizontalOptions = LayoutOptions.Start
            };
            button.Pressed += this.SearchByTag;

            TagLayout.Children.Add(button);
        }

        private void SearchByTag(object sender, System.EventArgs e)
        {
            string url = ((Button)sender).Text.ToString();//NovelFullConst.SearchByTag + ((Button)sender).CommandParameter;

            Navigation.PushAsync(new Pages.Novel.Catalog.ViewPage(url));
        }
    }
}