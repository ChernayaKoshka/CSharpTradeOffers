using System.Windows;

namespace SteamWebChat
{
    /// <summary>
    /// Interaction logic for LoadingScreen.xaml
    /// </summary>
    public partial class LoadingScreen
    {
        private readonly string _text;

        public LoadingScreen()
        {
            InitializeComponent();
        }

        public LoadingScreen(string text)
        {
            _text = text;
            InitializeComponent();
            Loaded += Form_Loaded;
        }

        void Form_Loaded(object sender, RoutedEventArgs e)
        {
            statusLabel.Content = _text;
        }
    }
}
