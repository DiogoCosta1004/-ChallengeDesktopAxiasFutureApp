using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Speech.Synthesis;
using System.Windows.Input;
using System.Windows.Threading;

namespace DesktopAxiasFutureApp.ViewModels
{
    public class FeedViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly WebSocketService _webSocketService;
        private readonly SpeechSynthesizer _synthesizer;
        private readonly Dispatcher _dispatcher;

        private bool _disposed;

        public ObservableCollection<string> Messages { get; }

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

        public ICommand ReadLastMessageCommand { get; }

        public FeedViewModel()
        {
            _webSocketService = new WebSocketService();
            _synthesizer = new SpeechSynthesizer();
            Messages = new ObservableCollection<string>();
            ReadLastMessageCommand = new RelayCommand(_ => ReadLastMessage());

            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        public async void Initialize()
        {
            _webSocketService.MessageReceived += HandleMessageReceived;
            _webSocketService.ErrorOccurred += HandleError;

            try
            {
                await _webSocketService.ConnectAsync("wss://edge-api.axiafutures.com/ws/?token=U2FsdGVkX1+YcfF5A506hKmuKwlK2a4WErOATfH/Ek9GtuMmtY0FbGqnH892r4B8");
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void HandleMessageReceived(string message)
        {
            _dispatcher.Invoke(() =>
            {
                Messages.Add(message);

                if (Messages.Count > 100)
                {
                    Messages.RemoveAt(0);
                }
            });
        }

        private void HandleError(Exception ex)
        {
            _dispatcher.Invoke(() =>
            {
                Messages.Add($"Erro: {ex.Message}");
            });
        }

        private void ReadLastMessage()
        {
            if (Messages.Count > 0)
            {
                _synthesizer.Volume = Volume;
                _synthesizer.SpeakAsync(Messages[^1]);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _webSocketService?.Dispose();
                    _synthesizer?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}