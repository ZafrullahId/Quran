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
            var ara_response = await _client.GetAsync($"editions/ara-quranacademy/{chapterNo}.json");
            int count = 1;
            if (ara_response.IsSuccessStatusCode)
            {
                var content = await ara_response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Domain.Quran>(content);
                var eng_result = await GetEngVersesByChapterNo(chapterNo);
                //result.Verses.Select(c => c.ArabicText += $"﴿{count}﴾").ToList();
                int i = 0;
                string arabicCount = string.Empty;
                foreach (var eng_verse in eng_result.Verses)
                {
                    arabicCount = count.ToString().ConvertNumerals();
                    result.Verses[i].Translation = eng_verse.ArabicText;
                    result.Verses[i].ArabicText += $"﴿{arabicCount}﴾";
                    i++;
                    count++;
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
        public async Task<string> GetAudioVerse(int chapterId, int verseNo)
        {
            var audioResponse = await _client.GetAsync($"https://api.alquran.cloud/v1/ayah/{chapterId}:{verseNo}/ar.alafasy");
            if (audioResponse.IsSuccessStatusCode)
            {
                var content = await audioResponse.Content.ReadAsStringAsync();
                dynamic response = JsonConvert.DeserializeObject<dynamic>(content);
                return response["data"]["audio"];
            }
            return null;
        }

    }
    public static class ArabicNumeralHelper
    {
        public static string ConvertNumerals(this string input)
        {
                return input.Replace('0', '\u06f0')
                        .Replace('1', '\u06f1')
                        .Replace('2', '\u06f2')
                        .Replace('3', '\u06f3')
                        .Replace('4', '٤')
                        .Replace('5', '\u06f5')
                        .Replace('6', '٦')
                        .Replace('7', '\u06f7')
                        .Replace('8', '\u06f8')
                        .Replace('9', '\u06f9');
        }
    }
}
