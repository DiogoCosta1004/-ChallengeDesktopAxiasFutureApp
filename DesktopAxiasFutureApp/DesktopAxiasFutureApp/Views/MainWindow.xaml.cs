using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Windows;

namespace DesktopAxiasFutureApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            var username = txtUsername.Text;
            var password = txtPassword.Password;

            var client = new HttpClient();
            var content = new StringContent(JsonSerializer.Serialize(new { username, password }), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://beta.axiafutures.com/api/mock-login", content);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Login successful!");
                var main = new FeedWindow(); // tela principal
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("\r\nError logging in. Please check your credentials.");
            }
        }
    }
}