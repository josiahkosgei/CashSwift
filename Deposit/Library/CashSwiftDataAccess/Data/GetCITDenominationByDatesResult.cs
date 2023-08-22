namespace CashSwiftDataAccess.Data
{
    public partial class GetCITDenominationByDates_Result
    {
        public string tx_currency { get; set; }
        public int denom { get; set; }
        public long? count { get; set; }
        public long? subtotal { get; set; }
    }
}
