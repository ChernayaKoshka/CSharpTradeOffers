using System.Windows;
using CSharpTradeOffers;

namespace SteamWebChat
{
    public class UserInputOutputHandler : IUserInputOutputHandler
    {
        public string GetInput()
        {
            return InputDialogBox.Show("Please input the requested field.", "Input");
        }

        public void OutputMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
