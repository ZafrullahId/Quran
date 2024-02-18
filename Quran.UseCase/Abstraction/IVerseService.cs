using Quran.Domain;

namespace Quran.UseCase.Abstraction
{
    public interface IVerseService
    {
        Task<List<VerseInfo>> GetVersesByChapterNoAsync(int chapterNo);
    }
}