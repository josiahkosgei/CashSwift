namespace CashSwift.API.Messaging.Authentication
{
    public class AuthenticationResponse : APIResponseBase
    {
        public bool IsInvalidCredentials { get; set; }
    }
}
