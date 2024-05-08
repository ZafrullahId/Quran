using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using Quran.ViewModels;

namespace Quran.Views;

public partial class ChapterAudioPlayerPage : ContentPage
{
    private readonly ChapterAudioPlayerViewModel viewModel;
    public ChapterAudioPlayerPage(ChapterAudioPlayerViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        this.BindingContext = viewModel;
        //this.viewModel.SetMediaElement(mediaElement);
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.viewModel.LoadChapters();
        await Task.Delay(1000);
        await this.viewModel.LoadSurahToPlay();

    }

    private async void MediaElement_MediaEnded(object sender, EventArgs e)
    {
        int chapterToPlayNext = this.viewModel.ChapterNo + 1;
        await this.viewModel.PlayQuranAudio(chapterToPlayNext);
        
    }

    private void MediaElement_MediaOpened(object sender, EventArgs e)
    {
        this.viewModel.SetPreferences();
    }

    //private void MediaElement_PositionChanged(object sender, MediaPositionChangedEventArgs e)
    //{
    //    viewModel.ResetMediaLastPlayedPosition();
    //}
}