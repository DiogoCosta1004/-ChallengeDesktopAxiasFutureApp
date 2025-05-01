using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DesktopAxiasFutureApp.Services
{
    public class WebSocketService : IDisposable
    {
        private readonly ClientWebSocket _socket = new();
        public event Action<string>? OnMessageReceived;

        public async Task ConnectAsync(string uri)
        {
            await _socket.ConnectAsync(new Uri(uri), CancellationToken.None);
            _ = ReceiveMessagesAsync();
        }

        private async Task ReceiveMessagesAsync()
        {
            var buffer = new byte[2048];
            while (_socket.State == WebSocketState.Open)
            {
                var result = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                if (message == "ping")
                {
                    await SendAsync("pong");
                }
                else
                {
                    OnMessageReceived?.Invoke(message);
                }
            }
        }

        public async Task SendAsync(string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            await _socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public void Dispose()
        {
            _socket?.Dispose();
        }
    }
}
