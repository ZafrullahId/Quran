using Quran.Domain;
using Quran.ViewModels;

namespace Quran.Views;

[QueryProperty(nameof(Chapter), "Chapter")]
public partial class VersesPage : ContentPage
{
	private readonly VersesViewModel versesViewModel;
	public VersesPage(VersesViewModel versesViewModel)
	{
		InitializeComponent();
		this.versesViewModel = versesViewModel;
		this.BindingContext = versesViewModel;
	}
    public Chapter Chapter { get; set; }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.versesViewModel.LoadVerses(Chapter.Id);
		title.Title = Chapter.ArabicName + "										  ";
    }
}