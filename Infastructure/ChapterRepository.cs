using Newtonsoft.Json;
using Quran.Domain;
using Quran.UseCase.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Infrastructure
{
    public class ChapterRepository : IChapterRepository
    {
        private readonly HttpClient _client;
        public ChapterRepository(HttpClient client)
        {
            _client = client;
        }
        public async Task<ChaptersInfo> GetChaptersInfo()
        {
            //var chaptersInfo = await _client.GetAsync("info.json");
            //if (chaptersInfo.IsSuccessStatusCode)
            //{
            //    var content = await chaptersInfo.Content.ReadAsStringAsync();
            //    var result = JsonConvert.DeserializeObject<ChaptersInfo>(content);
            //    return result;
            //}
            //return null;
            using var stream = await FileSystem.OpenAppPackageFileAsync("surahindex.json");
            using var reader = new StreamReader(stream);
            var contents = await reader.ReadToEndAsync();
            var chaptersInfo = JsonConvert.DeserializeObject<ChaptersInfo>(contents);
            return chaptersInfo;
        }
    }
}
