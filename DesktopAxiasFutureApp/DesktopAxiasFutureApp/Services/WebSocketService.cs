using DesktopAxiasFutureApp.Interfaces;
using System.Net.WebSockets;
using System.Text;

public class WebSocketService : IWebSocketService
{
    private ClientWebSocket _socket;
    private CancellationTokenSource _cts;

    public event Action<string> MessageReceived;
    public event Action<Exception> ErrorOccurred; 
    public event Action ConnectionClosed;

    public WebSocketService()
    {
        _socket = new ClientWebSocket();
        _cts = new CancellationTokenSource();
    }

    public async Task ConnectAsync(string uri)
    {
        try
        {
            await _socket.ConnectAsync(
                new Uri(uri),
                _cts.Token);

            _ = Task.Run(ReceiveMessages, _cts.Token);
        }
        catch (Exception ex)
        {
            ErrorOccurred?.Invoke(ex); 
            ConnectionClosed?.Invoke();
        }
    }

    private async Task ReceiveMessages()
    {
        var buffer = new byte[1024 * 4];

        while (_socket.State == WebSocketState.Open)
        {
            try
            {
                var result = await _socket.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    _cts.Token);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    if (message == "ping")
                    {
                        await SendPongAsync();
                    }
                    else
                    {
                        MessageReceived?.Invoke(message);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(ex); 
                ConnectionClosed?.Invoke();
                break;
            }
        }
    }

    private async Task SendPongAsync()
    {
        var pong = Encoding.UTF8.GetBytes("pong");
        await _socket.SendAsync(
            new ArraySegment<byte>(pong),
            WebSocketMessageType.Text,
            true,
            _cts.Token);
    }

    public void Dispose()
    {
        _cts?.Cancel();
        _socket?.Dispose();
        GC.SuppressFinalize(this);
    }
}