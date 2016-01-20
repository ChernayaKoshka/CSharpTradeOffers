namespace CSharpTradeOffers.Configuration
{
    public class ConfigErrorResult
    {
        public ConfigErrorResult()
        {

        }

        public ConfigErrorResult(bool valid)
        {
            Valid = valid;
        }

        public ConfigErrorResult(bool valid, string errorMessage)
        {
            Valid = valid;
            ErrorMessage = errorMessage;
        }

        public void AddError(string error)
        {
            Valid = false;
            ErrorMessage += error + "\r\n";
        }

        public bool Valid { get; private set; }

        public string ErrorMessage { get; private set; }
    }
}
