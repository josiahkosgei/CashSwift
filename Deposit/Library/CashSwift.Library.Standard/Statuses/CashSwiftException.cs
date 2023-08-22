// Statuses.CashSwiftException


using CashSwift.Library.Standard.Utilities;
using System;
using System.Runtime.Serialization;

namespace CashSwift.Library.Standard.Statuses
{
    [Serializable]
    public class CashSwiftException : Exception
    {
        public string PublicErrorCode { get; set; } = "500";

        public string PublicErrorMessage { get; set; } = "Error encountered. Kindly try again later or contact your administrator.";

        public string ServerErrorCode { get; set; } = "500";

        public string ServerErrorMessage { get; set; }

        public CashSwiftException()
        {
        }

        public CashSwiftException(string message)
          : base(message)
        {
            ServerErrorMessage = this.MessageString();
        }

        public CashSwiftException(string message, Exception inner)
          : base(message, inner)
        {
            ServerErrorMessage = this.MessageString();
        }

        protected CashSwiftException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }
    }
}
