using CommunityToolkit.Maui.Views;

namespace Quran.UseCase.Abstraction
{
    public interface IAudioService
    {
        void Stop();
        void Pause();
        void Forward();
        void Backward();
        void SkipForward();
        void SkipBackward();
        void SetPreferences();
        bool CheckAudioState();
        void PlayVerse(string audioUri);
        void ResetMediaLastPlayedPosition();
        void SetMediaElement(MediaElement mediaElement);
        bool Play(string audioUri, TimeSpan? position = null);
        Task<string> DownloadAudio(string audioUrl);
    }
}
