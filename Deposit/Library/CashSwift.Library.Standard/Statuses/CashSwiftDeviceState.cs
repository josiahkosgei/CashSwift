using System;

namespace CashSwift.Library.Standard.Statuses
{
    [Flags]
    public enum CashSwiftDeviceState
    {
        NONE = 0,
        DEVICE_MANAGER = 1,
        DATABASE = 2,
        PRINTER = 4,
        SERVER_CONNECTION = 8,
        CONTROLLER = 16,
        COUNTING_DEVICE = 32,
        BAG = 64,
        SAFE = 128,
        DEVICE_LOCK = 256, 
        ESCROW_JAM = 512,
        GUI_STARTUP_FAILED = 1024,
        LICENSE = 2048,
        HDD_FULL = 4096,
    }
}
