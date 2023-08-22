// Statuses.DeviceStatusChangedEventArgs


using System;

namespace CashSwift.Library.Standard.Statuses
{
    public class DeviceStatusChangedEventArgs : EventArgs
    {
        private ControllerStatus _controllerStatus;

        public DeviceStatusChangedEventArgs(
          ControllerStatus data,
          DeviceManagerState deviceManagerState = DeviceManagerState.NONE)
        {
            DeviceManagerState = deviceManagerState;
            _controllerStatus = data;
        }

        public DeviceManagerState DeviceManagerState { get; set; }

        public ControllerStatus ControllerStatus => _controllerStatus;
    }
}
