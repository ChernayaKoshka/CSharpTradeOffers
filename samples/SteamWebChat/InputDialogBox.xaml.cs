using System.Threading;
using System.Windows;

namespace SteamWebChat
{
    /// <summary>
    /// Interaction logic for InputDialogBox.xaml
    /// </summary>
    public partial class InputDialogBox
    {
        public InputDialogBox(string content, string title)
        {
            InitializeComponent();
            textBlock.Text = content;
            Title = title;
        }

        public string Result()
        {
            ShowDialog();
            return input.Text;
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
