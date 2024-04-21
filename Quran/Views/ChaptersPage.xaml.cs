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
        //chaptersViewModel.Media = mediaElement;
        //chaptersViewModel.MediaTogglePlayIcon = "play_icon.png";
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.chaptersViewModel.LoadChapters();
        await chaptersViewModel.GetAllPreferences();
    }

    //private void PlayButtonClicked(object sender, EventArgs e)
    //{
    //    if(chaptersViewModel.Media.CurrentState == CommunityToolkit.Maui.Core.Primitives.MediaElementState.Playing)
    //    {
    //        chaptersViewModel.Media.Pause();
    //        chaptersViewModel.MediaTogglePlayIcon = "play_icon.png";
    //    }
    //    else
    //    {
    //        chaptersViewModel.Media.Play();
    //        chaptersViewModel.MediaTogglePlayIcon = "pause.png";
    //    }

    //}
}