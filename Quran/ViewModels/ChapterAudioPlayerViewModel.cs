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

namespace Quran.ViewModels
{
    //[QueryProperty(nameof(ChapterNo), "ChapterId")]
    public partial class ChapterAudioPlayerViewModel : BaseViewModel
    {
        public ObservableCollection<Chapter> Chapters { get; }
        private readonly IChapterService chapterService;
        public ChapterAudioPlayerViewModel(IChapterService chapterService)
        {
            Chapters = [];
            this.chapterService = chapterService;
        }

        [ObservableProperty]
        int chapterNo;
        [ObservableProperty]
        MediaElement media;
        [ObservableProperty]
        double mediaProgress;
        public async Task LoadChapters()
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
            }
        }
        [RelayCommand]
        public async Task PlayQuranAudio(int chapterNo)
        {
            var chapter = Chapters.FirstOrDefault(x => x.Id == chapterNo);
            if (chapter is not null)
            {
                MediaProgress = (Media.Position.TotalSeconds / Media.Duration.TotalSeconds);
                Media.Source = $"https://cdn.islamic.network/quran/audio-surah/128/ar.alafasy/{chapterNo}.mp3";
                Preferences.Set("LastSurahPlayed", chapter.Name);
                ChapterNo = chapterNo;
            }
        }
        public async Task SetPrefrences()
        {

            Preferences.Set("IsMedialPlayedLast", true);
            Preferences.Set("LastPlayedTime", Media.Position.ToString());
            Preferences.Set("LastReciterPlayed", "Mishary Rashid Alafasy");
            Preferences.Set("LastSurahAudioLinkPlayed", Media.Source.ToString()[5..]);
        }
    }
}
