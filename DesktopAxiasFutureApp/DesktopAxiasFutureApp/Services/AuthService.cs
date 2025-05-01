using DesktopAxiasFutureApp.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DesktopAxiasFutureApp.Services
{
    public class AuthService
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<bool> LoginAsync(LoginModel model)
        {
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://beta.axiafutures.com/api/mock-login", content);
            return response.IsSuccessStatusCode;
        }
    }
}
