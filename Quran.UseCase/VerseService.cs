using Quran.Domain;
using Quran.UseCase.Abstraction;

namespace Quran.UseCase
{
    // All the code in this file is included in all platforms.
    public class VerseService : IVerseService
    {
        private readonly IVerseRepository _verseRepository;

        public VerseService(IVerseRepository verseRepository)
        {
            _verseRepository = verseRepository;
        }
        public async Task<List<VerseInfo>> GetVersesByChapterNoAsync(int chapterNo)
        {
            var verses = await _verseRepository.GetArabicVersesByChapterNo(chapterNo);
            return verses.Chapter;
        }
    }
}