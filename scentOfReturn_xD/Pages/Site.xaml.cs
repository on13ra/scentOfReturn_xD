namespace scentOfReturn_xD.Pages;

public partial class Site : ContentPage
{
	public Site()
	{
		InitializeComponent();
	}
    async private void ReturnBack(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }
}