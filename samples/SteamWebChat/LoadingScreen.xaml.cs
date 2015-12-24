using System;
using System.Threading;
using System.Windows;

namespace SteamWebChat
{
    /// <summary>
    /// Interaction logic for LoadingScreen.xaml
    /// </summary>
    public partial class LoadingScreen
    {
        private FriendsListWindow _mainWindow;

        public LoadingScreen()
        {
            InitializeComponent();
            Loaded += LoadingScreen_Loaded;
        }

        private void LoadingScreen_Loaded(object sender, RoutedEventArgs e)
        {
            _mainWindow = new FriendsListWindow();
            _mainWindow.OnLoadingFinished += MainWindow_LoadingFinished;
            _mainWindow.Closed += MainWindow_Closed;
            var loginThread = new Thread(() =>
            {
                _mainWindow.LoginAndGoOnline();
            });
            loginThread.Start();
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
    }
}
