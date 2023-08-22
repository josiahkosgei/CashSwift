namespace CashSwift.Finacle.Integration.Models.AccountValidation
{
    public class CheckAccountPermission_Result
    {
        public bool IsSuccess { get; set; }

        public string PublicErrorMessage { get; set; }

        public string ServerErrorMessage { get; set; }
    }
}
