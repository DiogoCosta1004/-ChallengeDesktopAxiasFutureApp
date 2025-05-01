using System.Net.WebSockets;
using System.Speech.Synthesis;
using System.Text;
using System.Windows;


namespace DesktopAxiasFutureApp
{
    public partial class FeedWindow : Window
    {
        private SpeechSynthesizer synthesizer = new SpeechSynthesizer();
        private ClientWebSocket socket;

        public FeedWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            socket = new ClientWebSocket();
            await socket.ConnectAsync(
                new Uri("wss://edge-api.axiafutures.com/ws/?token=U2FsdGVkX1+YcfF5A506hKmuKwlK2a4WErOATfH/Ek9GtuMmtY0FbGqnH892r4B8"),
                CancellationToken.None);

            var buffer = new byte[1024];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                if (message == "ping")
                {
                    var pong = Encoding.UTF8.GetBytes("pong");
                    await socket.SendAsync(new ArraySegment<byte>(pong), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                else
                {
                    Dispatcher.Invoke(() => lstMessages.Items.Add(message));
                }
            }
        }

        private void ReadLastMessage(object sender, RoutedEventArgs e)
        {
            if (lstMessages.Items.Count > 0)
            {
                var message = lstMessages.Items[^1].ToString();
                synthesizer.Volume = (int)volumeSlider.Value;
                synthesizer.SpeakAsync(message);
            }
        }
    }
}
