namespace scentOfReturn_xD.Pages;

public partial class GroupSelect : ContentPage
{
	public GroupSelect()
	{
		InitializeComponent();
	}

   async private void ReturnBack(object sender, TappedEventArgs e)
    {
		await Navigation.PopAsync();
    }
}