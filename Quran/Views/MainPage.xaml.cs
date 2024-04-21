using Quran.ViewModels;

namespace Quran.Views;

public partial class MainPage : ContentPage
{
	private readonly HomeViewModel _homeViewModel;
    public MainPage(HomeViewModel homeViewModel)
    {
        InitializeComponent();
        _homeViewModel = homeViewModel;
        BindingContext = _homeViewModel;
    }
}