
//BusinessObjects.Transactions.PostingProcCallResult


using System;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Transactions
{
    public class PostingProcCallResult
    {
        public Guid ID { get; set; }

        public Guid? PostingID { get; set; }

        public string Error { get; set; }
    }
}
