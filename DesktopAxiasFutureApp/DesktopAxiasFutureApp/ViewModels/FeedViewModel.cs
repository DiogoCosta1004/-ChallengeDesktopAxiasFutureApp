using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Speech.Synthesis;
using System.Windows.Input;
using DesktopAxiasFutureApp.Services;

namespace DesktopAxiasFutureApp.ViewModels
{
    public class FeedViewModel : INotifyPropertyChanged
    {
        private readonly WebSocketService _webSocketService = new();
        private readonly SpeechSynthesizer _synthesizer = new();
        public ObservableCollection<string> Messages { get; set; } = new();

        private int _volume = 100;
        public int Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                OnPropertyChanged(nameof(Volume));
            }
        }

        public ICommand ReadLastMessageCommand => new RelayCommand(_ => ReadLastMessage());

        public async void Initialize()
        {
            _webSocketService.OnMessageReceived += (msg) => Messages.Add(msg);
            await _webSocketService.ConnectAsync("wss://edge-api.axiafutures.com/ws/?token=U2FsdGVkX1+...");
        }

        private void ReadLastMessage()
        {
            if (Messages.Count > 0)
            {
                _synthesizer.Volume = Volume;
                _synthesizer.SpeakAsync(Messages[^1]);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
