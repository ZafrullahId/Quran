using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Quran.Domain;
using Quran.UseCase.Abstraction;
using CommunityToolkit.Maui.Views;
using Quran.UseCase.Constants;

namespace Quran.ViewModels
{
    public partial class ChapterAudioPlayerViewModel : BaseViewModel
    {
        public ObservableCollection<Chapter> Chapters { get; }
        private readonly IChapterService chapterService;
        private readonly IAudioService audioService;
        public ChapterAudioPlayerViewModel(IChapterService chapterService, IAudioService audioService)
        {
            Chapters = [];
            this.chapterService = chapterService;
            this.audioService = audioService;

        }

        [ObservableProperty]
        int chapterNo;
        [ObservableProperty]
        string mediaTogglePlayIcon;
        public async Task LoadChapters()
        {
            if (IsBusy || Chapters.Count == 114)
                return;
            try
            {
                IsBusy = true;
                var chapters = await this.chapterService.GetAllChapters();

                if (this.Chapters.Count != 0)
                    this.Chapters.Clear();

                foreach (var chapter in chapters)
                {
                    Chapters.Add(chapter);
                }
                var mediaIsPlaying = audioService.CheckAudioState();
                MediaTogglePlayIcon = !mediaIsPlaying ? "\u25b6" : "\u23f8";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to get surah index: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        [RelayCommand]
        public async Task PlayQuranAudio(int chapterNo)
        {
            if (chapterNo >= QuranConstant.TOTALNOSURAH)
                chapterNo = 0;
            var chapter = Chapters.FirstOrDefault(x => x.Id == chapterNo);
            if (chapter is not null)
            {
                this.audioService.Stop();
                this.audioService.Play($"https://cdn.islamic.network/quran/audio-surah/128/ar.alafasy/{chapterNo}.mp3");
                Preferences.Set("LastSurahPlayed", chapter.Name);
                Preferences.Set("LastSurahAudioLinkPlayed", $"https://cdn.islamic.network/quran/audio-surah/128/ar.alafasy/{chapterNo}.mp3");
                ChapterNo = chapterNo;
            }
        }
        public void SetPreferences()
        {
            this.audioService.SetPreferences();
        }
        public void ResetMediaLastPlayedPosition()
        {
            this.audioService.ResetMediaLastPlayedPosition();
        }
        [RelayCommand]
        public async Task PlayAudio()
        {
            if (Preferences.ContainsKey("LastSurahAudioLinkPlayed"))
            {
                var recentAudioSource = Preferences.Get("LastSurahAudioLinkPlayed", "");
                var currentMediaPosition = Preferences.Get("CurrentMediaPosition", "");
                if (!string.IsNullOrEmpty(currentMediaPosition))
                {
                    if (TimeSpan.TryParse(currentMediaPosition, out TimeSpan mediaPos))
                    {
                        var isPlayed = this.audioService.Play(recentAudioSource, mediaPos);
                        if (isPlayed)
                            MediaTogglePlayIcon = "\u23f8";
                        else
                            MediaTogglePlayIcon = "\u25b6";
                    }
                }
            }
        }
        [RelayCommand]
        public async Task Foward_15Sec()
        {
            this.audioService.Forward();
        }
        [RelayCommand]
        public async Task Backward_15Sec()
        {
            this.audioService.Backward();
        }
        [RelayCommand]
        public async Task SkipForward()
        {
            this.audioService.SkipForward();
        }
        [RelayCommand]
        public async Task SkipBackward()
        {
            this.audioService.SkipBackward();
        }
        public async Task LoadSurahToPlay()
        {
            var playing = audioService.CheckAudioState();
            if (playing)
            {
                MediaTogglePlayIcon = "\u23f8";
                return;
            }
            if (Preferences.ContainsKey("LastSurahAudioLinkPlayed"))
            {
                var recentAudioSource = Preferences.Get("LastSurahAudioLinkPlayed", "");
                var currentMediaPosition = Preferences.Get("CurrentMediaPosition", "");
                if (!string.IsNullOrEmpty(currentMediaPosition))
                {
                    if (TimeSpan.TryParse(currentMediaPosition, out TimeSpan mediaPos))
                        this.audioService.Play(recentAudioSource, mediaPos);
                }
            }
            var mediaIsPlayng = audioService.CheckAudioState();
            MediaTogglePlayIcon = !mediaIsPlayng ? "\u25b6" : "\u23f8";
        }
        [RelayCommand]
        public async Task DownloadSurahAudio(int chapterNo)
        {
            var url = string.Format("https://cdn.islamic.network/quran/audio-surah/128/ar.alafasy/{0}.mp3", chapterNo);
            await this.audioService.DownloadAudio(url);

        }
    }
}
