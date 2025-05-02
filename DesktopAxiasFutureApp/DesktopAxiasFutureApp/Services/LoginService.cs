using System.Net.Http;
using System.Text;
using DesktopAxiasFutureApp.Interfaces;

namespace DesktopAxiasFutureApp.Services
{
    public class LoginService : ILoginService
    {
        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var request = new HttpRequestMessage(
                        HttpMethod.Post,
                        "https://beta.axiafutures.com/api/mock-login");

                    request.Content = new StringContent(
                        $"{{\"username\":\"{username}\",\"password\":\"{password}\"}}",
                        Encoding.UTF8,
                        "application/json");

                    var response = await client.SendAsync(request);
                    return response.IsSuccessStatusCode;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}