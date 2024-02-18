using Newtonsoft.Json;

namespace Quran.Domain
{
    public class VerseInfo
    {
        [JsonProperty("chapter")]
        public int ChapterNo { get; set; }
        [JsonProperty("verse")]
        public int VerseNo { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        public string Translation { get; set; }
    }
}
