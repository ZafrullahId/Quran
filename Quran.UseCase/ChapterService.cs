using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Quran.Domain;
using Quran.UseCase.Abstraction;

namespace Quran.UseCase
{
    public class ChapterService : IChapterService
    {
        private readonly IChapterRepository _chapterRepository;
        private IMemoryCache _cache;
        public ChapterService(IChapterRepository chapterRepository, IMemoryCache cache)
        {
            _chapterRepository = chapterRepository;
            _cache = cache;
        }
        public async Task<List<Chapter>> GetAllChapters()
        {
            if (_cache.TryGetValue("chapters", out IEnumerable<Chapter> chapters))
                return chapters.ToList();

            var chaptersInfo = await _chapterRepository.GetChaptersInfo();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                    .SetPriority(CacheItemPriority.Normal)
                    .SetSize(1024);
            _cache.Set("chapters", chaptersInfo.Chapters, cacheEntryOptions);

            return chaptersInfo.Chapters;
        }
    }
}
