using Quran.ViewModels;

namespace Quran.Views;

public partial class SearchPage : ContentPage
{
    private readonly SearchViewModel _viewModel;
    public SearchPage(SearchViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        this.BindingContext = _viewModel;
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(1000);
        searchSurah.Focus();
    }
}