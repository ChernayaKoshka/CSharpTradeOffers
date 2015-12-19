using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SteamWebChat
{
    /// <summary>
    /// Interaction logic for InputDialogBox.xaml
    /// </summary>
    public partial class InputDialogBox
    {
        public InputDialogBox()
        {
            InitializeComponent();
        }

        public static string Show(string content, string title)
        {
            var inputDialogBox = new InputDialogBox
            {
                textBlock = {Text = content},
                Title = title
            };

            inputDialogBox.ShowDialog();

            return inputDialogBox.input.Text;
        }
    }
}
