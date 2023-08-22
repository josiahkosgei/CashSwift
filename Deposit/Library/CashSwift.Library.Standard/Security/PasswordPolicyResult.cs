// Security.PasswordPolicyResult


namespace CashSwift.Library.Standard.Security
{
    public enum PasswordPolicyResult
    {
        PASSED,
        MINIMUM_LENGTH,
        UPPER_CASE_LENGTH,
        LOWER_CASE_LENGTH,
        SPECIAL_LENGTH,
        NUMERIC_LENGTH,
        HISTORY,
    }
}
