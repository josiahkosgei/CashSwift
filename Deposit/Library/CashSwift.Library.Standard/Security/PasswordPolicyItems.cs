// Security.PasswordPolicyItems


namespace CashSwift.Library.Standard.Security
{
    public struct PasswordPolicyItems
    {
        public int Minimum_Length;
        public int Upper_Case_length;
        public int Lower_Case_length;
        public int Special_length;
        public int Numeric_length;
        public int HistorySize;
        public bool Use_History;
    }
}
