using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Domain
{
    public class Chapter
    {
        [JsonProperty("chapter")]
        public int Id { get; set; }
        public string Name {  get; set; }
        public string ArabicName {  get; set; }
        public string EnglishName {  get; set; }
    }
    public class ChaptersInfo
    {
        public List<Chapter> Chapters { get; set;}
    }
}
