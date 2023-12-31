﻿namespace CashSwift.Finacle.Integration.Models.FundsTransfer
{
    public class FundsTransferRequest
    {
        public string SystemCode_Cred { get; set; }

        public string BankID { get; set; }

        public string CDM_Number { get; set; }
        public string SystemCode_FT { get; set; }

        public string TransactionType { get; set; }

        public string TransactionSubType { get; set; }

        public string TransactionReference_Dr { get; set; }

        public string TransactionItemKey_Dr { get; set; }

        public string AccountNumber_Dr { get; set; }

        public decimal TransactionAmount_Dr { get; set; }

        public string TransactionCurrency_Dr { get; set; }

        public string Narrative_Dr { get; set; }

        public string TransactionReference_Cr { get; set; }

        public string TransactionItemKey_Cr { get; set; }

        public string AccountNumber_Cr { get; set; }

        public decimal TransactionAmount_Cr { get; set; }

        public string TransactionCurrency_Cr { get; set; }

        public string Narrative_Cr { get; set; }
    }
}
