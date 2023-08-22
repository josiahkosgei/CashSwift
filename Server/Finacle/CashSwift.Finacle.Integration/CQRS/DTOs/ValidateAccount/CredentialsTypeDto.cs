﻿namespace CashSwift.Finacle.Integration.CQRS.DTOs.ValidateAccount
{
    public class CredentialsTypeDto
    {
        public CredentialsTypeDto()
        {
            BankID = "01";
        }

        public string SystemCode { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string BankID { get; set; }
    }
}