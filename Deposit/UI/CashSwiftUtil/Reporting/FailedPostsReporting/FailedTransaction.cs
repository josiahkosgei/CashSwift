﻿
// Type: CashSwiftUtil.Reporting.FailedPostsReporting.FailedTransaction
// Assembly: CashSwiftUtil, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 885F1C6C-21D2-4135-B89E-154B0975A233
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashSwiftDeposit\App\UI\6.0\CashSwiftUtil.dll

using CashSwiftUtil.Reporting.MSExcel;
using System;

namespace CashSwiftUtil.Reporting.FailedPostsReporting
{
    public class FailedTransaction
    {
        [EpplusIgnore]
        public Guid id { get; set; }

        [EpplusIgnore]
        public Guid session_id { get; set; }

        [EpplusIgnore]
        public Guid? CIT { get; set; }

        [EpplusIgnore]
        public bool notes_rejected { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string DeviceName { get; set; }

        public string Type { get; set; }

        public string DeviceTransactionNumber { get; set; }

        public string Currency { get; set; }

        public Decimal? Amount { get; set; }

        public string AccountNumber { get; set; }

        public string AccountName { get; set; }

        public string ReferenceAccountNumber { get; set; }

        public string ReferenceAccountName { get; set; }

        public string Narration { get; set; }

        public string DepositorName { get; set; }

        public string IDNumber { get; set; }

        public string Phone { get; set; }

        public string FundsSource { get; set; }

        public bool NoteJamDetected { get; set; }

        public bool EscrowJamDetected { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string CB_TransactionReference { get; set; }

        public DateTime? CB_Date { get; set; }

        public string CB_Status { get; set; }

        public string CB_StatusDetail { get; set; }

        public int Result { get; set; }
    }
}
