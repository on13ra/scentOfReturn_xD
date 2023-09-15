using Microsoft.Maui.Controls.Internals;
using scentOfReturn_xD.Pages;
using System.Text;
using System.Net.Sockets;
using System.Net;
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
            string message = ;
            byte[] data = Encoding.UTF8.GetBytes(message);
            var client = new UdpClient();
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000); // endpoint where server is listening
            client.Connect(ep);
            // send data
            client.Send(data);
            // then receive data
            var receivedData = client.Receive(ref ep);
            string line = Encoding.UTF8.GetString(receivedData);
            Console.WriteLine(line);
        }
    }
}