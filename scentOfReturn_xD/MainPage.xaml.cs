using Microsoft.Maui.Controls.Internals;
using scentOfReturn_xD.Pages;
using Xamarin.Essentials;

namespace scentOfReturn_xD

{

    public partial class MainPage : ContentPage
    {
        //static string tempPath = System.IO.Path.GetTempPath();
        public MainPage()
        {
            InitializeComponent();

            double width = Microsoft.Maui.Devices.DeviceDisplay.MainDisplayInfo.Width;
            double almostwidth = width * 0.1;
            
        }

        async private void ToNews(object sender, TappedEventArgs e)
        {
            await Navigation.PushAsync(new Site());
        }

        async private void ToFeatures(object sender, TappedEventArgs e)
        {
            await Navigation.PushAsync(new Features());
        }

        async private void SelectGroup(object sender, TappedEventArgs e)
        {
            await Navigation.PushAsync(new GroupSelect());
        }

        private void updateRasp(object sender, EventArgs e)
        {

        }
    }
}