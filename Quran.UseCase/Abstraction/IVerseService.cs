using Quran.Domain;

namespace Quran.UseCase.Abstraction
{
    public interface IVerseService
    {
        Task<string> GetAudioVerse(int chapterId, int verseNo);
        Task<List<VerseInfo>> GetVersesByChapterNoAsync(int chapterNo);
    }
}