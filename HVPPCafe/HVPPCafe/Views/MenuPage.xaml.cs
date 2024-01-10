using HVPPCafe.ViewModels;

namespace HVPPCafe.Views;

public partial class MenuPage : ContentPage
{
	public MenuPage()
	{
		InitializeComponent();
		BindingContext = new MenuViewModel();
	}
}