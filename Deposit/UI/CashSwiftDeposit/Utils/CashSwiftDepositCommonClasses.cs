using CashSwiftDataAccess.Data;
using CashSwiftDataAccess.Entities;
using System;

namespace CashSwiftDeposit.Utils
{
    internal static class CashSwiftDepositCommonClasses
    {
        internal static DeviceStatus GenerateDeviceStatus(
          Guid deviceID,
          DepositorDBContext DBContext)
        {
            if (DBContext == null)
                throw new NullReferenceException("dBContext is null at GenerateDeviceStatus");
            return new DeviceStatus()
            {
                device_id = deviceID,
                machine_name = Environment.MachineName.ToUpperInvariant(),
                bag_note_capacity = 0.ToString() ?? "",
                bag_note_level = 0,
                bag_number = "N/A",
                bag_percent_full = 0,
                bag_status = "N/A",
                bag_value_capacity = new long?(0L),
                bag_value_level = new long?(0L),
                ba_currency = "N/A",
                ba_status = "N/A",
                ba_type = "N/A",
                controller_state = "N/A",
                current_status = 1024,
                escrow_position = "N/A",
                escrow_status = "N/A",
                escrow_type = "N/A",
                id = Guid.NewGuid(),
                modified = new DateTime?(DateTime.Now),
                sensors_status = "N/A",
                sensors_type = "N/A",
                sensors_value = 0,
                sensors_bag = "N/A",
                sensors_door = "N/A"
            };
        }
    }
}
