using System.Threading.Tasks;

namespace DesktopAxiasFutureApp.Interfaces
{
    public interface ILoginService
    {
        Task<bool> AuthenticateAsync(string username, string password);
    }
}