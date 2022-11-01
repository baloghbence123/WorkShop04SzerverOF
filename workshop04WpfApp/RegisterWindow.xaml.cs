using System;
using System.Net.Http;
using System.Windows;

namespace workshop04WpfApp
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        RegisterViewModel model = new RegisterViewModel();
        public RegisterWindow()
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

            var response = await client.PutAsJsonAsync<RegisterViewModel>("auth", model);
            model.Password = tb_password.Password;
            model.UserName = tb_email.Text;

            if (response.IsSuccessStatusCode)
            {
                var result = MessageBox.Show("Registration succesful", "Info", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                this.DialogResult = true;
            }
        }
    }

    public class RegisterViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }

    }
}
