using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Quran.Domain;
using Quran.UseCase.Abstraction;
using Quran.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.ViewModels
{
    public partial class ChapterViewModel : BaseViewModel
    {
        [ObservableProperty]
        bool isMedialPlayedLast;
        [ObservableProperty]
        string mediaTogglePlayIcon;
        [ObservableProperty]
        bool isLastReadChapterSet;

        [ObservableProperty]
        Chapter lastReadChapter;
        public ObservableCollection<Chapter> Chapters { get; }
        private readonly IChapterService chapterService;
        private readonly IAudioService audioService;
        public ChapterViewModel(IChapterService chapterService, IAudioService audioService)
        {
            Chapters = [];
            this.chapterService = chapterService;
            Title = "Surah Index";
            this.audioService = audioService;
        }
        public async Task GetAllPreferences()
        {
            var mediaState = audioService.CheckAudioState();
            MediaTogglePlayIcon = mediaState ? "\u23f8" : "\u25b6";
            IsMedialPlayedLast = Preferences.Get("IsMedialPlayedLast", false);
            if (Preferences.ContainsKey("LastReadSurahId") && Preferences.Get("LastReadSurahId", 0) != 0)
            {
                int lasReadSurahId = Preferences.Get("LastReadSurahId", 0);
                LastReadChapter = Chapters.FirstOrDefault(x => x.Id == lasReadSurahId);
                IsLastReadChapterSet = true;
            }
        }
        [RelayCommand]
        public async Task GotToChapterVerses(Chapter chapter)
        {
            if (chapter is null) return;

            var navigationParameter = new Dictionary<string, object> { { "Chapter", chapter } };

            await Shell.Current.GoToAsync(nameof(VersesPage), navigationParameter);
        }

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
        public async Task GoToChapterAudioPlayerPage(int chapterNo)
        {
            if (chapterNo == 0) return;
            await Shell.Current.GoToAsync($"{nameof(ChapterAudioPlayerPage)}?ChapterId={chapterNo}");
        }
        [RelayCommand]
        public async Task RefreshChapters()
        {
            if (IsBusy)
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
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to get surah index: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }
        [RelayCommand]
        public async Task GoToSearchPage()
        {
            await Shell.Current.GoToAsync(nameof(SearchPage));
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
        private async Task GoToAudioPage()
        {
            await Shell.Current.GoToAsync(nameof(ChapterAudioPlayerPage));
        }
    }
}

