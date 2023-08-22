
//BusinessObjects.Transactions.TransactionPostingStatus


namespace CashSwiftCashControlPortal.Module.BusinessObjects.Transactions
{
    public enum TransactionPostingStatus
    {
        CREATED,
        INITIATED,
        AUTH_AWAIT,
        AUTHORISED,
        POSTING,
        COMPLETE,
        REJECT,
        ERROR,
    }
}
