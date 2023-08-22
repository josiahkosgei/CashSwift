using CashSwift.Library.Standard.Statuses;

namespace CashSwiftDeposit.Utils
{
    public class FormField
    {
        public string DataEntryLabel { get; set; }

        public string DataEntryTextbox { get; set; }

        public string ErrorTextBlock { get; set; }

        public KeyboardType KeyboardType { get; set; }

        public object DataItem { get; set; }
    }
}
