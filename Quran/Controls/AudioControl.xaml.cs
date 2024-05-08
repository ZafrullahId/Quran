using CommunityToolkit.Maui.Core.Primitives;
using Quran.Views;
using System.Runtime.CompilerServices;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace Quran.Controls;

public partial class AudioControl : ContentView
{

    public AudioControl()
    {
        InitializeComponent();
    }
    //protected override async void OnChildAdded(Element child)
    //{
    //    base.OnChildAdded(child);
    //    await Task.Delay(1000);
    //    playedReciter.Text = Preferences.Get("LastReciterPlayed", "");
    //    palyedSurah.Text = Preferences.Get("LastSurahPlayed", "");
    //    if (Preferences.ContainsKey("LastSurahAudioLinkPlayed"))
    //    {
    //        mediaElement.Source = Preferences.Get("LastSurahAudioLinkPlayed", "");
    //        var currentMediaPosition = Preferences.Get("CurrentMediaPosition", "");
    //        if (!string.IsNullOrEmpty(currentMediaPosition))
    //        {
    //            if (TimeSpan.TryParse(currentMediaPosition, out TimeSpan mediaPos))
    //               await mediaElement.SeekTo(mediaPos);
    //        }
    //    }
    //}
    //protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    //{
    //    base.OnPropertyChanged(propertyName);
    //    PlayButton.SetBinding(Button.CommandProperty, "PlayAudioCommand");
    //}
    private void PlayButton_Clicked(object sender, EventArgs e)
    {

        PlayButton.SetBinding(Button.CommandProperty, "PlayAudioCommand");
        //if (mediaElement.CurrentState is MediaElementState.Playing)
        //{
        //    mediaElement.Pause();
        //    PlayButton.Source = "play_icon.png";
        //    return;
        //}
        //PlayButton.Source = "pause.png";
        //mediaElement.Play();
    }

    //private void PlayBackButton_Clicked(object sender, EventArgs e)
    //{
    //    mediaElement.SeekTo(mediaElement.Position.Subtract(TimeSpan.FromSeconds(10)));
    //}

    //private void MediaElement_PositionChanged(object sender, MediaPositionChangedEventArgs e)
    //{
    //    playedTime.Text = mediaElement.Position.ToString(@"hh\:mm\:ss");
    //    Preferences.Set("CurrentMediaPosition", mediaElement.Position.ToString());
    //}

    //private void PlayForwardButton_Clicked(object sender, EventArgs e)
    //{
    //    mediaElement.SeekTo(mediaElement.Position.Add(TimeSpan.FromSeconds(10)));
    //}

    //private async void AudioSub_Tapped(object sender, TappedEventArgs e)
    //{
    //    mediaElement.Pause();
    //    await Shell.Current.GoToAsync(nameof(ChapterAudioPlayerPage));
    //}
}