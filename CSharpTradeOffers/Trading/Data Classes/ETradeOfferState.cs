namespace CSharpTradeOffers.Trading
{
    public enum ETradeOfferState
    {
        ETradeOfferStateInvalid = 1,

        ETradeOfferStateActive = 2,

        ETradeOfferStateAccepted = 3,

        ETradeOfferStateCountered = 4,

        ETradeOfferStateExpired = 5,

        ETradeOfferStateCanceled = 6,

        ETradeOfferStateDeclined = 7,

        ETradeOfferStateInvalidItems = 8,

        ETradeOfferStateEmailPending = 9,

        ETradeOfferStateEmailCanceled = 10
    }
}