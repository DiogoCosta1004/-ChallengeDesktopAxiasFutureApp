namespace DesktopAxiasFutureApp.Interfaces
{
    public interface ITextToSpeechService : IDisposable
    {
        void Speak(string text);
        void SetVolume(int volume);
        void Stop();
    }
}