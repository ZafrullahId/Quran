using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
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
            chaptersInfo.Chapters = chaptersInfo.Chapters.Select(x => new Chapter
            {
                Id = x.Id,
                Name = x.Name,
                ArabicName = x.ArabicName,
                EnglishName = x.EnglishName,
                Revelation = x.Revelation,
                NoOfAyahs = x.Verses.Count,
            }).ToList();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                    .SetPriority(CacheItemPriority.Normal)
                    .SetSize(1024);
            _cache.Set("chapters", chaptersInfo.Chapters, cacheEntryOptions);

            return chaptersInfo.Chapters;
        }
        public async Task<List<Chapter>> GetAllChapterByFilterText(string filterText)
        {

            var chaptersInfo = await _chapterRepository.GetChaptersInfo();
            var chapters = chaptersInfo.Chapters.Where(x => 
            x.Name.StartsWith(filterText,StringComparison.OrdinalIgnoreCase) || 
            x.ArabicName.StartsWith(filterText, StringComparison.OrdinalIgnoreCase) ||
            x.EnglishName.StartsWith(filterText)).ToList();
            return chapters;
        }
        public async Task<Chapter> GetChapterAsync(int id)
        {
            if (_cache.TryGetValue("chapters", out IEnumerable<Chapter> chapters))
                return chapters.Where(x => x.Id == id).FirstOrDefault();
            return null;
        }
    }
}
