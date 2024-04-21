using CommunityToolkit.Mvvm.Input;
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
        public HomeViewModel()
        {
            Title = "Home";
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
