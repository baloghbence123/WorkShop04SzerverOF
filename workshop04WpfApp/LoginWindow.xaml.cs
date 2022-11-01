using System;
using System.Net.Http;
using System.Windows;

namespace workshop04WpfApp
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public static string Path = "";
        public LoginWindow()
        {
            InitializeComponent();


        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5073");
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
            );

            var response = await client.PostAsJsonAsync<LoginViewModel>("auth", new LoginViewModel()
            {
                UserName = tb_usr.Text,
                Password = tb_pw.Text
            });

            var token = await response.Content.ReadAsAsync<TokenModel>();
            token.Expiration = token.Expiration.ToLocalTime();

            Path = tb_path.Text;
            MainWindow mainWindow = new MainWindow(token);
            mainWindow.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RegisterWindow rw = new RegisterWindow();
            rw.ShowDialog();
        }
    }

    public class TokenModel
    {
        public string Token { get; set; }

        public DateTime Expiration { get; set; }
    }

    internal class LoginViewModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
