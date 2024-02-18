using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.Domain
{
    public class Quran
    {
        public List<VerseInfo> Chapter { get; set; }
    }
}
