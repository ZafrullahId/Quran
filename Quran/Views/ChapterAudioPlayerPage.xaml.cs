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
        this.viewModel.Media = mediaElement;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.viewModel.LoadChapters();

    }

    private void MediaElement_MediaEnded(object sender, EventArgs e)
    {
        if (this.viewModel.ChapterNo >= 114)
            this.viewModel.ChapterNo = 0;
        else
            this.viewModel.ChapterNo++;

        this.viewModel.Media.Source = $"https://cdn.islamic.network/quran/audio-surah/128/ar.alafasy/{this.viewModel.ChapterNo}.mp3";
        this.viewModel.Media.Play();
    }

    private async void mediaElement_MediaOpened(object sender, EventArgs e)
    {
        await this.viewModel.SetPrefrences();
    }

}