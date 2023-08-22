// Statuses.ControllerStateChangedEventArgs


using System;

namespace CashSwift.Library.Standard.Statuses
{
    public class ControllerStateChangedEventArgs : EventArgs
    {
        private ControllerState _data;

        public ControllerStateChangedEventArgs(ControllerState data) => _data = data;

        public ControllerState ControllerState => _data;
    }
}
