using Quran.Domain;
using Quran.ViewModels;

namespace Quran.Views;

public partial class VersesPage : ContentPage
{
    private readonly VersesViewModel _versesViewModel;
    public VersesPage(VersesViewModel versesViewModel)
    {
        InitializeComponent();
        _versesViewModel = versesViewModel;
        this.BindingContext = versesViewModel;
    }
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _versesViewModel.SetPreferences().GetAwaiter();
        if (Preferences.ContainsKey("LastReadVerse") && Preferences.Get("LastReadVerse", 0) == _versesViewModel.Surah.Id)
        {
            int lastReadVerse = Preferences.Get("LastReadVerse", 0);
            await Task.Delay(3000);
            VerseInfo verseInfo = _versesViewModel.Verses.Where(x => x.VerseNo == lastReadVerse).FirstOrDefault();
            versesCollection.ScrollTo(verseInfo);
        }
    }
}