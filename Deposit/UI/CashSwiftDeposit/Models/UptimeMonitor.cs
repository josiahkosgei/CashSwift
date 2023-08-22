using CashSwift.Library.Standard.Statuses;
using CashSwift.Library.Standard.Utilities;
using CashSwiftDataAccess.Data;
using CashSwiftDataAccess.Entities;
using CashSwiftDeposit.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace CashSwiftDeposit.Models
{
    internal class UptimeMonitor
    {
        private static UptimeMonitor _uptimeMonitor = new UptimeMonitor();

        public static UptimeModeType CurrentUptimeMode { get; private set; }

        public static CashSwiftDeviceState CurrentUptimeComponentState { get; private set; }

        private UptimeMonitor()
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                DateTime now = DateTime.Now;
                DbSet<UptimeMode> uptimeModes = DBContext.UptimeModes;
                Expression<Func<UptimeMode, bool>> predicate1 = x => !x.end_date.HasValue;
                foreach (UptimeMode uptimeMode in (IEnumerable<UptimeMode>)uptimeModes.Where(predicate1))
                    uptimeMode.end_date = new DateTime?(now);
                DbSet<UptimeComponentState> uptimeComponentStates = DBContext.UptimeComponentStates;
                Expression<Func<UptimeComponentState, bool>> predicate2 = x => !x.end_date.HasValue;
                foreach (UptimeComponentState uptimeComponentState in (IEnumerable<UptimeComponentState>)uptimeComponentStates.Where(predicate2))
                    uptimeComponentState.end_date = new DateTime?(now);
                ApplicationViewModel.SaveToDatabase(DBContext);
            }
        }

        public UptimeMonitor GetInstance()
        {
            if (UptimeMonitor._uptimeMonitor == null)
                UptimeMonitor._uptimeMonitor = new UptimeMonitor();
            return UptimeMonitor._uptimeMonitor;
        }

        public static void SetCurrentUptimeMode(UptimeModeType state)
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                try
                {
                    if (state == UptimeMonitor.CurrentUptimeMode)
                        return;
                    DateTime now = DateTime.Now;
                    Device device = ApplicationViewModel.GetDevice(DBContext);
                    UptimeMonitor.CurrentUptimeMode = state;
                    UptimeMode uptimeMode = DBContext.UptimeModes.Where(x => x.device == device.id).OrderByDescending(x => x.created).FirstOrDefault();
                    if (uptimeMode != null)
                        uptimeMode.end_date = new DateTime?(now);
                    DBContext.UptimeModes.Add(new UptimeMode()
                    {
                        id = GuidExt.UuidCreateSequential(),
                        device = device.id,
                        created = now,
                        start_date = now,
                        device_mode = (int)state
                    });
                    ApplicationViewModel.SaveToDatabase(DBContext);
                }
                catch (Exception ex)
                {
                    ApplicationViewModel.Log.Error(nameof(UptimeMonitor), "Error", nameof(SetCurrentUptimeMode), ex.MessageString(), Array.Empty<object>());
                }
            }
        }

        public static void SetCurrentUptimeComponentState(CashSwiftDeviceState state)
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                try
                {
                    if (UptimeMonitor.CurrentUptimeComponentState.HasFlag(state))
                        return;
                    DateTime now = DateTime.Now;
                    Device device = ApplicationViewModel.GetDevice(DBContext);
                    UptimeMonitor.CurrentUptimeComponentState = state;
                    if (DBContext.UptimeComponentStates.Where(x => x.device == device.id && x.component_state == (int)state && !x.end_date.HasValue).OrderByDescending(x => x.created).FirstOrDefault() == null)
                        DBContext.UptimeComponentStates.Add(new UptimeComponentState()
                        {
                            id = GuidExt.UuidCreateSequential(),
                            device = device.id,
                            created = now,
                            start_date = now,
                            component_state = (int)state
                        });
                    ApplicationViewModel.SaveToDatabase(DBContext);
                }
                catch (Exception ex)
                {
                    ApplicationViewModel.Log.Error(nameof(UptimeMonitor), "Error", nameof(SetCurrentUptimeComponentState), ex.MessageString(), Array.Empty<object>());
                }
            }
        }

        public static void UnSetCurrentUptimeComponentState(CashSwiftDeviceState state)
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                try
                {
                    if (!UptimeMonitor.CurrentUptimeComponentState.HasFlag(state))
                        return;
                    DateTime now = DateTime.Now;
                    Device device = ApplicationViewModel.GetDevice(DBContext);
                    UptimeMonitor.CurrentUptimeComponentState = state;
                    UptimeComponentState entity = DBContext.UptimeComponentStates.Where(x => x.device == device.id && x.component_state == (int)state && !x.end_date.HasValue).OrderByDescending(x => x.created).FirstOrDefault();
                    if (entity != null)
                    {
                        if (now - entity.start_date < TimeSpan.FromMinutes(1.0))
                            DBContext.UptimeComponentStates.Remove(entity);
                        else
                            entity.end_date = new DateTime?(now);
                    }
                    ApplicationViewModel.SaveToDatabase(DBContext);
                }
                catch (Exception ex)
                {
                    ApplicationViewModel.Log.Error(nameof(UptimeMonitor), "Error", nameof(UnSetCurrentUptimeComponentState), ex.MessageString(), Array.Empty<object>());
                }
            }
        }
    }
}
