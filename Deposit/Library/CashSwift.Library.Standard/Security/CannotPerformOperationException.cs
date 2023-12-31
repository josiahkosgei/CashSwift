﻿// Security.CannotPerformOperationException


using System;

namespace CashSwift.Library.Standard.Security
{
    internal class CannotPerformOperationException : Exception
    {
        public CannotPerformOperationException()
        {
        }

        public CannotPerformOperationException(string message)
          : base(message)
        {
        }

        public CannotPerformOperationException(string message, Exception inner)
          : base(message, inner)
        {
        }
    }
}
