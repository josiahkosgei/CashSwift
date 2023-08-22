using CashSwift.Library.Standard.Statuses;
using System;

namespace DeviceManager
{
    public class ConnectionEventArgs : EventArgs
    {
        private StandardResult _data;

        public ConnectionEventArgs(StandardResult data) => _data = data;

        public StandardResult Data => _data;
    }
}
