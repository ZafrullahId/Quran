using Quran.Domain;
using Quran.UseCase.Abstraction;
using System.Collections.ObjectModel;

namespace Quran.ViewModels;

public partial class VersesViewModel : ContentView
{
	private readonly IVerseService _verseService;
    public VersesViewModel(IVerseService verseService)
    {
        InitializeComponent();
        _verseService = verseService;
        this.Chapter = new ObservableCollection<VerseInfo>();
    }
    public string ChapterName { get; set; }
    public ObservableCollection<VerseInfo> Chapter { get; set; }
    public async Task LoadVerses(int chapterNo)
    {
        this.Chapter.Clear();
        var chapter = await _verseService.GetVersesByChapterNoAsync(chapterNo);
        foreach (var verse in chapter)
        {
            this.Chapter.Add(verse);
        }
    }
}