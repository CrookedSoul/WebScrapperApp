using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraperApp.LocalData.Entity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.IO;

namespace WebScraperApp.Pages.User
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserProfilePage : ContentPage
    {
        private UserInfo entUser;
        string PhotoPath;
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }

        public UserProfilePage(UserInfo ent)
        {
            InitializeComponent();

            this.entUser = ent;

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            (Application.Current.MainPage as MasterDetailPage).IsGestureEnabled = true;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            (Application.Current.MainPage as MasterDetailPage).IsGestureEnabled = false;
        }

        async Task TakePhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                await LoadPhotoAsync(photo);
                Console.WriteLine($"CapturePhotoAsync COMPLETED: {PhotoPath}");
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature is not supported on the device
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }

        async Task LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                PhotoPath = null;
                return;
            }
            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

            PhotoPath = newFile;
        }

        private async void MediaPickerBtn_Clicked(object sender, EventArgs e)
        {
            MediaPickerOptions options = new MediaPickerOptions();
            options.Title = "Please pick you profile image";
            var result = await MediaPicker.PickPhotoAsync(options);

            var stream = await result.OpenReadAsync();

            if (entUser == null)
            {
                entUser = new UserInfo();
            }

            // Create bytearray for User profile image
            MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);
            entUser.ProfileImage = ms.ToArray();

            pickedImage.Source = ImageSource.FromStream(() => new MemoryStream(entUser.ProfileImage));
        }

        private async void SignUpBtn_Clicked(object sender, EventArgs e)
        {
            if (entUser == null)
            {
                entUser = new UserInfo();
            }
            entUser.UserName = UserNameEntry.Text;

            await App.UserDB.SaveAsync(entUser);

            RootPage.RefreshDetailPage();

            //await Navigation.PushAsync(new Novel.CatalogPage(null));
        }

        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

    }
}