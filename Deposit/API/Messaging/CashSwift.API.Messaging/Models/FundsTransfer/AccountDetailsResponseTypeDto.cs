using System;

namespace CashSwift.API.Messaging.Models
{
    public class AccountDetailsResponseTypeDto
    {
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string CurrencyCode { get; set; }
        public string JointAccount { get; set; }
        public string ProductID { get; set; }
        public string ProductContextCode { get; set; }
        public string ProductName { get; set; }
        public string BookedBalance { get; set; }
        public string BlockedBalance { get; set; }
        public string AvailableBalance { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string PhoneNumber { get; set; }
        public string CustomerCode { get; set; }
        public string Email { get; set; }
        public string Dormant { get; set; }
        public string Stopped { get; set; }
        public string Closed { get; set; }
        public string AccountRightsIndicator { get; set; }
        public string PostalAddress { get; set; }
        public string Town { get; set; }
        public DateTime OpenDate { get; set; }
        public string Status { get; set; }
    }
}