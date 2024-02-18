using CommunityToolkit.Mvvm.Input;
using Quran.Domain;
using Quran.UseCase.Abstraction;
using Quran.Views;
using System.Collections.ObjectModel;

namespace Quran.ViewModels;

public partial class ChaptersViewModel : ContentView
{
	private readonly IChapterService chapterService;
    public ChaptersViewModel(IChapterService chapterService)
    {
        InitializeComponent();
        this.chapterService = chapterService;
        this.Chapters = new ObservableCollection<Chapter>();
    }
    public ObservableCollection<Chapter> Chapters { get; set; }
    public async Task LoadChapters()
    {
        this.Chapters.Clear();
        var chapters = await this.chapterService.GetAllChapters();
        foreach ( var chapter in chapters )
        {
            Chapters.Add(chapter);
        }
    }
    public async Task GotToChaperVerses(Chapter chapter)
    {
        var navigationParameter = new Dictionary<string, object> { { "Chapter", chapter } };

        await Shell.Current.GoToAsync(nameof(VersesPage), navigationParameter);
    }
}