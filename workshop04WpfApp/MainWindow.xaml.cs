using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using WorkShop04.Models;

namespace workshop04WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<FileModel> FileModels { get; set; }
        UserInfo UserInfo { get; set; }
        FileSystemWatcher FileSystemWatcher;
        TokenModel tokenModel;
        HttpClient client;

        public MainWindow(TokenModel token)
        {
            InitializeComponent();

            FileSystemWatcher = new FileSystemWatcher();
            FileSystemWatcher.Path = LoginWindow.Path;

            FileSystemWatcher.Changed += Create;
            FileSystemWatcher.Deleted += Create;
            FileSystemWatcher.Created += Create;

            FileSystemWatcher.Filter = "*.*";
            FileSystemWatcher.EnableRaisingEvents = true;

            System.Threading.Thread.Sleep(3000);

            client = new HttpClient();
            this.tokenModel = token;

            client.BaseAddress = new Uri("http://localhost:5073");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
              new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Token);

            Task.Run(async () =>
            {
                FileModels = new ObservableCollection<FileModel>(await GetFiles());
                UserInfo = await GetUserInfo();
            }).Wait();

            this.DataContext = this;
        }

        async Task Refresh()
        {
            FileModels = new ObservableCollection<FileModel>(await GetFiles());
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FileModels"));
        }
        private async void Create(object sender, FileSystemEventArgs e)
        {
            FileModel actualFileModel = new FileModel();
            actualFileModel.Name = "asdasd";
            actualFileModel.Path = FileSystemWatcher.Path;
            var response = await client.PostAsJsonAsync("/File", actualFileModel);
            await Refresh();
        }

        async Task<IEnumerable<FileModel>> GetFiles()
        {
            var response = await client.GetAsync("/File");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<FileModel>>();
            }
            throw new Exception("something wrong...");
        }

        async Task<UserInfo> GetUserInfo()
        {
            var response = await client.GetAsync("auth");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<UserInfo>();
            }
            throw new Exception("something wrong...");
        }



    }
    public class UserInfo
    {
        public string Email { get; set; }
        public List<string> Roles { get; set; }

    }
}
