using CommunityToolkit.Mvvm.ComponentModel;

namespace HVPPCafe.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        string title;
    }
}
