using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using Quran.UseCase.Abstraction;

namespace Quran.UseCase
{
    public class AudioService : IAudioService
    {
        private MediaElement _mediaElement;
        public AudioService()
        {
            _mediaElement = new MediaElement();
        }
        public void Play(int chapterIdToPlay)
        {
            _mediaElement = new MediaElement();
            _mediaElement.Source = $"https://cdn.islamic.network/quran/audio-surah/128/ar.alafasy/{chapterIdToPlay}.mp3"; ;
            _mediaElement.Play();
        }
    }
}
