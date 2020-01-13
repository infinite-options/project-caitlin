using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using static Android.Support.V4.Media.MediaBrowserCompat;

namespace ImageCapture
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Take_Pic(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsTakePhotoSupported && !CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Message", "Photo capture and Pick not supported", "ok");
                return;
            } else
            {
                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Directory = "Images",
                    Name = DateTime.Now + "_pic.jpg"

                });

                if (file == null)
                    return;

                await DisplayAlert("File Path", file.Path, "Ok");
                MyImage.Source = ImageSource.FromStream(() =>

                {

                    var stream = file.GetStream();
                    return stream;

                });
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("No Upload", "Picking photo is not supported", "Ok");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
                return;
            MyImage.Source = ImageSource.FromStream(() => file.GetStream());

        }
    }
}
