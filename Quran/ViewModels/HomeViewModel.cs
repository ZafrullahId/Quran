using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using Quran.UseCase.Abstraction;
using Quran.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.ViewModels
{
    public partial class HomeViewModel : BaseViewModel
    {
        private readonly IAudioService _audioService;
        public HomeViewModel(IAudioService audioService)
        {
            Title = "Home";
            _audioService = audioService;
        }
        public void SetMediaElement(MediaElement element)
        {
            _audioService.SetMediaElement(element);
        }
        [RelayCommand]
        public async Task GoToChaptersPage()
        {
            await Shell.Current.GoToAsync(nameof(ChaptersPage));
        }
        [RelayCommand]
        public async Task GoToChapterAudioPage()
        {
            await Shell.Current.GoToAsync(nameof(ChapterAudioPlayerPage));
        }
    }
}
