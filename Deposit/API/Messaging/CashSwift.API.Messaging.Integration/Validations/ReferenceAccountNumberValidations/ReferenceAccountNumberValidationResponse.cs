using CashSwift.API.Messaging.Models;

namespace CashSwift.API.Messaging.Integration.Validations.ReferenceAccountNumberValidations
{
    public class ReferenceAccountNumberValidationResponse : ValidationResponseBase
    {
        public string RequestedReferenceAccountNumber { get; set; }
    }
}
