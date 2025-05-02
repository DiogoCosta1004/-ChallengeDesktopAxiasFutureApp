using System.Speech.Synthesis;
using DesktopAxiasFutureApp.Interfaces;

namespace DesktopAxiasFutureApp.Services
{
    public class TextToSpeechService : ITextToSpeechService
    {
        private readonly SpeechSynthesizer _synthesizer;

        public TextToSpeechService()
        {
            _synthesizer = new SpeechSynthesizer();
            _synthesizer.Volume = 50; 
        }

        public void Speak(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                _synthesizer.SpeakAsyncCancelAll(); 
                _synthesizer.SpeakAsync(text);
            }
        }

        public void Stop()
        {
            _synthesizer.SpeakAsyncCancelAll();
        }

        public void SetVolume(int volume)
        {
            _synthesizer.Volume = Math.Max(0, Math.Min(volume, 100));
        }

        public void Dispose()
        {
            _synthesizer.Dispose();
        }
    }
}
