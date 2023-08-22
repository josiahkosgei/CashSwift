using System;
using System.Runtime.Serialization;

namespace CashSwift.API.Messaging.Integration.Exceptions
{
    [Serializable]
    internal class CashSwiftAPIValidationException : Exception
    {
        public CashSwiftAPIValidationException()
        {
        }

        public CashSwiftAPIValidationException(string message)
          : base(message)
        {
        }

        public CashSwiftAPIValidationException(string message, Exception innerException)
          : base(message, innerException)
        {
        }

        protected CashSwiftAPIValidationException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }
    }
}
