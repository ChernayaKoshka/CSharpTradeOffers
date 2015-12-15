namespace CSharpTradeOffers
{
    public interface IUserInputOutputHandler
    {
        string GetInput();
        void OutputMessage(string message);
    }
}
