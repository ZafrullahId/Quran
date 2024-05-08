using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Quran.Domain;
using Quran.UseCase.Abstraction;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.ViewModels
{
    [QueryProperty(nameof(Chapter), "Chapter")]
    public partial class VersesViewModel : BaseViewModel
    {
        public ObservableCollection<VerseInfo> Verses { get; }
        private readonly IVerseService _verseService;
        private readonly IChapterService _chapterService;
        private readonly IAudioService _audioService;
        public VersesViewModel(IVerseService verseService, IChapterService chapterService, IAudioService audioService)
        {
            _verseService = verseService;
            Verses = [];
            _chapterService = chapterService;
            _audioService = audioService;
        }
        [ObservableProperty]
        Chapter surah;
        [ObservableProperty]
        bool isChapterHasBismillah;
        public Chapter Chapter
        {
            set
            {
                if (value is not null)
                {
                    Surah = value;
                    int chapterNo = value.Id;
                    Title = value.ArabicName;
                    IsChapterHasBismillah = value.Id != 9;
                    LoadVerses(chapterNo).GetAwaiter();
                }
            }
        }
        public async Task SetPreferences()
        {
            Preferences.Set("LastReadSurahId", Surah.Id);
        }
        public async Task LoadVerses(int chapterNo)
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                var chapter = await _verseService.GetVersesByChapterNoAsync(chapterNo);

                if (this.Verses.Count != 0)
                    Verses.Clear();

                foreach (var verse in chapter)
                {
                    this.Verses.Add(verse);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to Load surah verses: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        [RelayCommand]
        public async Task GoToNextChapterVerses(int id)
        {
            int nextSurah = 0;
            if (id >= 114)
                nextSurah = 1;
            else
                nextSurah = id + 1;

            var chapter = await _chapterService.GetChapterAsync(nextSurah);
            if (chapter is null)
            {
                await Shell.Current.DisplayAlert("Error!", $"Unable to Load next surah", "OK");
            }
            Chapter = chapter;
            Preferences.Set("LastReadSurahId", Surah.Id);
        }
        [RelayCommand]
        public async Task PlayVerse(int verseNo)
        {
            try
            {
                var audioLink = await _verseService.GetAudioVerse(Surah.Id, verseNo);
                if (audioLink is not null)
                {
                    //this._audioService.Stop();
                    this._audioService.PlayVerse(audioLink);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", $"Unable to load audio: check your connection", "OK");
            }
        }
        [RelayCommand]
        public async Task CopyQuranVerse(VerseInfo copyVerse)
        {
            await Clipboard.Default.SetTextAsync($"{copyVerse.ArabicText}\n\n{copyVerse.Translation}\n\nSurah: {Surah.Name}, Verse {copyVerse.VerseNo}");
        }
    }

}
