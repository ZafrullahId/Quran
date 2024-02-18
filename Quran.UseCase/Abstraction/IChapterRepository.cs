using Quran.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.UseCase.Abstraction
{
    public interface IChapterRepository
    {
        Task<ChaptersInfo> GetChaptersInfo();
    }
}
