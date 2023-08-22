namespace CashSwift.API.Messaging.Models
{
    public class ResponseHeader
    {
        public string CorrelationID { get; set; }
        public string MessageID { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public StatusMessages StatusMessages { get; set; }
    }


}
