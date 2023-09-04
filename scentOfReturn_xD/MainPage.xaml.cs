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

        async private void SelectGroup(object sender, TappedEventArgs e)
        {
            await Navigation.PushAsync(new GroupSelect());
        }

        private void HAJKATuE(object sender, EventArgs e)
        {

        }
    }
}