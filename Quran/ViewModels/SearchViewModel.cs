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
    public partial class SearchViewModel : BaseViewModel
    {
        private readonly IChapterService _chapterService;
        public ObservableCollection<Chapter> Chapters { get; set; }
        public SearchViewModel(IChapterService chapterService)
        {
            _chapterService = chapterService;
            Chapters = [];
        }
        private string filterText;

        public string FilterText
        {
            get { return filterText; }
            set
            {
                filterText = value;
                LoadFilteredListAsync(filterText).GetAwaiter();
            }
        }
        public async Task LoadFilteredListAsync(string filterText)
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                Chapters.Clear();
                if (string.IsNullOrWhiteSpace(filterText))
                {
                    return;
                }
                else
                {
                    var chapters = await _chapterService.GetAllChapterByFilterText(filterText);
                    foreach (var chapter in chapters)
                    {
                        this.Chapters.Add(chapter);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to get search: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        public async Task LoadChapters()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                var chapters = await _chapterService.GetAllChapters();

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
    }
}
