using scentOfReturn_xD.Pages;

namespace scentOfReturn_xD
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

       

        async private void ToNews(object sender, TappedEventArgs e)
        {
            await Navigation.PushAsync(new Site());
        }

        async private void ToFeatures(object sender, TappedEventArgs e)
        {
            await Navigation.PushAsync(new Features());
        }
    }
}