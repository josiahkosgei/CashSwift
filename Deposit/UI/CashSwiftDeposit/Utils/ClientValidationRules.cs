using System.Text.RegularExpressions;

namespace CashSwiftDeposit.Utils
{
    public static class ClientValidationRules
    {
        public static bool RegexValidation(string input, string regularExpression) => Regex.Match(input, regularExpression, RegexOptions.IgnoreCase).Success;
    }
}
