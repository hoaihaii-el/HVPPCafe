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
}
