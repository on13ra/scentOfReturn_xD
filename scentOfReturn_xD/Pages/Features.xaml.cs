namespace scentOfReturn_xD.Pages;

public partial class Features : ContentPage
{
	public Features()
	{
		InitializeComponent();
	}

   async private void ReturnBack(object sender, TappedEventArgs e)
    {
		await Navigation.PopAsync();
    }
}