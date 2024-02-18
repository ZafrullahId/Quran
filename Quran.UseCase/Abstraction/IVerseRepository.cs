using Quran.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quran.UseCase.Abstraction
{
    public interface IVerseRepository
    {
        Task<Domain.Quran> GetEngVersesByChapterNo(int chapterNo);
        Task<Domain.Quran> GetArabicVersesByChapterNo(int chapterNo);
    }
}
