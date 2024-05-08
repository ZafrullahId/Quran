using CommunityToolkit.Maui.Views;
using Quran.Domain;
using Quran.ViewModels;

namespace Quran.Views;

public partial class ChaptersPage : ContentPage
{
    private readonly ChapterViewModel chaptersViewModel;
    public ChaptersPage(ChapterViewModel chaptersViewModel)
    {
        InitializeComponent();
        this.chaptersViewModel = chaptersViewModel;
        this.BindingContext = chaptersViewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.chaptersViewModel.LoadChapters();
        await chaptersViewModel.GetAllPreferences();
        playedReciter.Text = Preferences.Get("LastReciterPlayed", "");
        playedSurah.Text = Preferences.Get("LastSurahPlayed", "");
    }
}