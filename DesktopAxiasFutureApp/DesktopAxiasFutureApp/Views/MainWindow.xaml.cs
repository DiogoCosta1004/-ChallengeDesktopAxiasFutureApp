using DesktopAxiasFutureApp.Services;
using DesktopAxiasFutureApp.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;

namespace DesktopAxiasFutureApp
{
    public partial class MainWindow : Window
    {
        private readonly ILoginService _loginService;
        private readonly IWebSocketService _webSocketService;
        private readonly ITextToSpeechService _ttsService;

        public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();

            _loginService = new LoginService();
            _webSocketService = new WebSocketService();
            _ttsService = new TextToSpeechService();

            lstMessages.ItemsSource = Messages;

            SetupWebSocket();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var username = txtUsername.Text;
            var password = txtPassword.Password;

            var isAuthenticated = await _loginService.AuthenticateAsync(username, password);

            if (isAuthenticated)
            {
                lblLoginStatus.Text = "Login bem-sucedido!";

                await _webSocketService.ConnectAsync("wss://edge-api.axiafutures.com/ws/?token=U2FsdGVkX1+YcfF5A506hKmuKwlK2a4WErOATfH/Ek9GtuMmtY0FbGqnH892r4B8");

            }
            else
            {
                lblLoginStatus.Text = "Falha no login!";
            }
        }

        private void SetupWebSocket()
        {
            _webSocketService.MessageReceived += message =>
            {
                Dispatcher.Invoke(() =>
                {
                    Messages.Add(message);
                    lstMessages.ScrollIntoView(lstMessages.Items[lstMessages.Items.Count - 1]);
                });
            };

            _webSocketService.ConnectionClosed += () =>
            {
                Dispatcher.Invoke(() =>
                {
                    lblConnectionStatus.Text = "Conexão perdida";
                });
            };
        }

        private void btnRead_Click(object sender, RoutedEventArgs e)
        {
            if (lstMessages.SelectedItem is string selectedMessage)
            {
                _ttsService.Speak(selectedMessage);
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            _ttsService.Stop();
        }

        private void sldVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_ttsService != null)
            {
                _ttsService.SetVolume((int)e.NewValue);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _webSocketService.Dispose();
            _ttsService.Dispose();
            base.OnClosed(e);
        }
    }
}