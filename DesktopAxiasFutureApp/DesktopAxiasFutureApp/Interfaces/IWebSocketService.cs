using System;

namespace DesktopAxiasFutureApp.Interfaces
{
    public interface IWebSocketService : IDisposable
    {
        event Action<string> MessageReceived;
        event Action<Exception> ErrorOccurred;
        event Action ConnectionClosed;
        Task ConnectAsync(string uri);

    }
}