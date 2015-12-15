using System;

namespace CSharpTradeOffers
{
    public class ConsoleInputOutput : IUserInputOutputHandler
    {
        public string GetInput()
        {
            return Console.ReadLine();
        }

        public void OutputMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
