using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Storage;
using Quran.UseCase.Abstraction;
using Quran.UseCase.Constants;
using System;

namespace Quran.UseCase
{
    public class AudioService : IAudioService
    {
        private MediaElement mediaPlayer;
        private static int playingSurahId;
        private bool keepPositionTrack = true;
        private readonly HttpClient _client;
        public AudioService(HttpClient client)
        {
            _client = client;
        }

        public void SetMediaElement(MediaElement mediaElement)
        {
            this.mediaPlayer = mediaElement;
            this.mediaPlayer.PositionChanged += MediaPlayer_PositionChanged;
            this.mediaPlayer.MediaEnded += MediaPlayer_Ended;
        }
        public bool Play(string audioUri, TimeSpan? position = null)
        {
            keepPositionTrack = true;
            if (mediaPlayer != null)
            {
                if (mediaPlayer.CurrentState is MediaElementState.Playing)
                {
                    Pause();
                    return false;
                }
                else if (mediaPlayer.CurrentState is MediaElementState.Paused)
                {
                    mediaPlayer.Play();
                    return true;
                }
                string filePath = GenerateAudioFilePath(audioUri);
                if (File.Exists(filePath))
                {
                    // Play the audio file
                    mediaPlayer.Source = filePath;
                    mediaPlayer.Play();
                    var part = filePath.Split("/").LastOrDefault().Split(".");
                    var id = int.Parse((part[1][^1]).ToString());
                    playingSurahId = id;
                    return true;
                }
                mediaPlayer.Source = MediaSource.FromUri(new Uri(audioUri));
                if (position != null && position.HasValue)
                    mediaPlayer.SeekTo(position.Value);

                mediaPlayer.Play();
                string[] parts = audioUri.Split('/');
                string chapterPart = parts[parts.Length - 1];
                playingSurahId = int.Parse(chapterPart.Split(".").FirstOrDefault());
                return true;
            }
            return false;
        }
        public void PlayVerse(string audioUri)
        {
            if (mediaPlayer != null)
            {
                mediaPlayer.Source = MediaSource.FromUri(new Uri(audioUri));
                mediaPlayer.Play();
                keepPositionTrack = false;
            }
        }
        public void Pause()
        {
            mediaPlayer?.Pause();
        }

        public void Stop()
        {
            mediaPlayer?.Stop();
        }
        public void Forward()
        {
            mediaPlayer?.SeekTo(mediaPlayer.Position.Add(TimeSpan.FromSeconds(15)));
        }
        public void Backward()
        {
            mediaPlayer?.SeekTo(mediaPlayer.Position.Subtract(TimeSpan.FromSeconds(15)));
        }
        public void SkipForward()
        {
            Stop();
            if (mediaPlayer != null)
            {
                if (playingSurahId >= QuranConstant.TOTALNOSURAH)
                    playingSurahId = 1;
                else
                    playingSurahId++;

                Play($"https://cdn.islamic.network/quran/audio-surah/128/ar.alafasy/{playingSurahId}.mp3");
            }
        }
        public void SkipBackward()
        {
            Stop();
            if (mediaPlayer != null)
            {
                if (playingSurahId <= 1)
                    playingSurahId = 1;
                else
                    playingSurahId--;

                Play($"https://cdn.islamic.network/quran/audio-surah/128/ar.alafasy/{playingSurahId}.mp3");
            }
        }
        public void SetPreferences()
        {
            Preferences.Set("IsMedialPlayedLast", true);
            Preferences.Set("LastReciterPlayed", "Mishary Rashid Alafasy");
            Preferences.Set("LastSurahAudioLinkPlayed", mediaPlayer.Source.ToString()[5..]);
        }
        public void ResetMediaLastPlayedPosition()
        {
            Preferences.Set("CurrentMediaPosition", mediaPlayer.Position.ToString());
        }
        private void MediaPlayer_PositionChanged(object sender, MediaPositionChangedEventArgs e)
        {
            if (keepPositionTrack)
                ResetMediaLastPlayedPosition();
        }
        private void MediaPlayer_Ended(object sender, EventArgs e)
        {
            if (keepPositionTrack)
                SkipForward();
        }
        public bool CheckAudioState() => mediaPlayer.CurrentState == MediaElementState.Playing;

        public async Task<string> DownloadAudio(string audioUrl)
        {
            try
            {
                // Download the audio file from the URL
                byte[] audioBytes = await _client.GetByteArrayAsync(audioUrl);

                // Save the audio file to local storage
                string filePath = GenerateAudioFilePath(audioUrl);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                await File.WriteAllBytesAsync(filePath, audioBytes);

                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading audio: {ex.Message}");
                return null;
            }
        }
        private string GenerateAudioFilePath(string audioUrl)
        {
            // Create a unique filename for the downloaded audio
            string[] parts = audioUrl.Split('/');
            string fileName = string.Join("", parts[^2], parts[^1]);
            string filePath = Path.Combine(FileSystem.CacheDirectory, QuranConstant.AudioCacheFolder, fileName);
            return filePath;

        }
    }
}
