using Microsoft.Extensions.Caching.Memory;
using Quran.Domain;
using Quran.UseCase.Abstraction;
using System.Threading;

namespace Quran.UseCase
{
    // All the code in this file is included in all platforms.
    public class VerseService : IVerseService
    {
        private static readonly SemaphoreSlim GetUsersSemaphore = new SemaphoreSlim(1, 1);
        private readonly IVerseRepository _verseRepository;
        private readonly IMemoryCache _cache;

        public VerseService(IVerseRepository verseRepository, IMemoryCache cache)
        {
            _verseRepository = verseRepository;
            _cache = cache;
        }
        public async Task<List<VerseInfo>> GetVersesByChapterNoAsync(int chapterNo)
        {
            bool isAvailable = _cache.TryGetValue($"chapter{chapterNo}verses", out List<VerseInfo> verses);
            if (isAvailable) return verses;
            try
            {
                await GetUsersSemaphore.WaitAsync();
                isAvailable = _cache.TryGetValue($"chapter{chapterNo}verses", out verses);
                if (isAvailable) return verses;
                var chapter = await _verseRepository.GetArabicVersesByChapterNo(chapterNo);
                verses = chapter.Verses;
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMonths(6),
                    Size = 10000,
                };
                _cache.Set($"chapter{chapterNo}verses", verses, cacheEntryOptions);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                GetUsersSemaphore.Release();
            }
            return verses;
        }
        public async Task<string> GetAudioVerse(int chapterId, int verseNo)
        {
            var audioLink = await _verseRepository.GetAudioVerse(chapterId, verseNo);
            return audioLink ?? null;
        }
    }
}