
//BusinessObjects.Transactions.PostingAuthResult


using System;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Transactions
{
    public class PostingAuthResult
    {
        public Guid ID { get; set; }

        public Guid? PostingID { get; set; }

        public string Error { get; set; }
    }
}
