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
