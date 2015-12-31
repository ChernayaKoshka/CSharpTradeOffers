using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CSharpTradeOffers.Configuration;
using CSharpTradeOffers.Web;

namespace SteamWebChat
{
    /// <summary>
    /// Interaction logic for LoadingScreen.xaml
    /// </summary>
    public partial class LoadingScreen
    {
        private FriendsListWindow _mainWindow;

        private static readonly XmlConfigHandler ConfigHandler = new XmlConfigHandler("configuration.xml");
        private static readonly Web Web = new Web(new SteamWebRequestHandler());

        public LoadingScreen()
        {
            InitializeComponent();
            Loaded += LoadingScreen_Loaded;
        }

        private void LoadingScreen_Loaded(object sender, RoutedEventArgs e)
        {

            #region config
            var config = ConfigHandler.Reload();
            if (string.IsNullOrEmpty(config.ApiKey))
                ComplainQuit("API key is missing. Please fill in the API key field in \"configuration.xml\"");
            if (string.IsNullOrEmpty(config.Username) || string.IsNullOrEmpty(config.Password))
                ComplainQuit(
                    "Username or Password missing. Please fill in their respective fields in \"configuration.xml\"");
            #endregion

            #region login
            Task.Run(() =>
            {
                Account account = Web.RetryDoLogin(TimeSpan.FromSeconds(5), 10, config.Username, config.Password, config.SteamMachineAuth, new UserInputOutputHandler());

                if (!string.IsNullOrEmpty(account.SteamMachineAuth))
                {
                    config.SteamMachineAuth = account.SteamMachineAuth;
                    ConfigHandler.WriteChanges(config);
                }

                Dispatcher.Invoke(() =>
                {
                    _mainWindow = new FriendsListWindow(account, config.ApiKey);
                    _mainWindow.OnLoadingFinished += MainWindow_LoadingFinished;
                    _mainWindow.Closed += MainWindow_Closed;
                    var loginThread = new Thread(() =>
                    {
                        _mainWindow.LoginAndGoOnline();
                    });
                    loginThread.Start();
                });
            });
            #endregion
        }

        void MainWindow_LoadingFinished(object sender, EventArgs e)
        {
            Dispatcher.Invoke(Hide);
            Dispatcher.Invoke(() => { _mainWindow.Visibility = Visibility.Visible; });
        }

        void MainWindow_Closed(object sender, EventArgs e)
        {
            Close();
        }

        void ComplainQuit(string message, string title = "Error")
        {
            MessageBox.Show(message, title);
            Close();
        }
    }
}
