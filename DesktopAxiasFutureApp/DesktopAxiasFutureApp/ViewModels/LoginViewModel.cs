using DesktopAxiasFutureApp.Models;
using DesktopAxiasFutureApp.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace DesktopAxiasFutureApp.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly LoginService _authService = new();
        public LoginModel Credentials { get; set; } = new();

        public RelayCommand LoginCommand => new(async _ => await LoginAsync());

        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Credentials.Username) || string.IsNullOrWhiteSpace(Credentials.Password))
            {
                MessageBox.Show("Usuário e senha são obrigatórios.");
                return;
            }

            var success = await _authService.AuthenticateAsync(Credentials.Username, Credentials.Password);

            if (success)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var feedWindow = new FeedWindow();
                    feedWindow.Show();
                    Application.Current.MainWindow?.Close();
                });
            }
            else
            {
                MessageBox.Show("Credenciais inválidas.");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
