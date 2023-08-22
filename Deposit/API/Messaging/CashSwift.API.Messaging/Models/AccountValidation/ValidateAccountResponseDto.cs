using System;

namespace CashSwift.API.Messaging.Models
{
    public class AccounValidationResponseDto
    {
        public EnvelopeDto Envelope { get; set; }
        public bool IsSuccess { get; set; }
        public string PublicErrorMessage { get; set; }
        public string PublicErrorCode { get; set; }
    }


    public class EnvelopeDto
    {
        public HeaderDto Header { get; set; }
        public BodyDto Body { get; set; }
    }

    public class HeaderDto
    {
        public Responseheader ResponseHeader { get; set; }
    }

    public class Responseheader
    {
        public string CorrelationID { get; set; }
        public string MessageID { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public Statusmessages StatusMessages { get; set; }
    }

    public class Statusmessages
    {
        public string MessageCode { get; set; }
        public string MessageDescription { get; set; }
        public object MessageType { get; set; }
    }

    public class BodyDto
    {
        public AccountDetailsResponseTypeDto AccountDetailsResponse { get; set; }
    }

}
