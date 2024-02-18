using Newtonsoft.Json;
using Quran.Domain;
using Quran.UseCase.Abstraction;
using System.Net.Http;

namespace Quran.Infrastructure
{
    // All the code in this file is included in all platforms.
    public class VerseRepository : IVerseRepository
    {
        private readonly HttpClient _client;
        public VerseRepository(HttpClient client)
        {
            _client = client;
        }
        public async Task<Domain.Quran> GetArabicVersesByChapterNo(int chapterNo)
        {
            var ara_response = await _client.GetAsync($"editions/ara-quranbazzi/{chapterNo}.json");
            if (ara_response.IsSuccessStatusCode)
            {
                var content = await ara_response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Domain.Quran>(content);
                var eng_result = await GetEngVersesByChapterNo(chapterNo);
                int i = 0;
                foreach (var eng_verse in eng_result.Chapter)
                {
                    while (true)
                    {
                        result.Chapter[i].Translation = eng_verse.Text;
                        result.Chapter[i].Text += "۞";
                        break;
                    }
                    i++;
                }
                return result;
            }

            return null;
        }
        public async Task<Domain.Quran> GetEngVersesByChapterNo(int chapterNo)
        {
            var eng_response = await _client.GetAsync($"editions/eng-ahmedali/{chapterNo}.json");
            if (eng_response.IsSuccessStatusCode)
            {
                var content = await eng_response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Domain.Quran>(content);
                return result;
            }

            return null;
        }
        
    }
}