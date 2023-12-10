namespace HVPPCafe;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        if (DeviceInfo.Idiom == DeviceIdiom.Phone)
        {
            this.CurrentItem = PhoneTabs;
        }
    }

    private void Button_Focused(object sender, FocusEventArgs e)
    {
        Button1.Opacity = 1;
    }

    private void Button_Unfocused(object sender, FocusEventArgs e)
    {
        Button1.Opacity = 1;
    }
}
