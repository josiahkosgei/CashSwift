using CashSwift.API.CDM.Messaging.Reporting.Uptime;
using CashSwift.API.CDM.Messaging.Reporting.Uptime.Models.Messaging;
using CashSwift.Library.Standard.Statuses;
using CashSwift.Library.Standard.Utilities;
using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF;
using CashSwiftCashControlPortal.Module.BusinessObjects.CDM_API;
using CashSwiftCashControlPortal.Module.BusinessObjects.Devices;
using CashSwiftCashControlPortal.Module.BusinessObjects.Server;
using CashSwiftCashControlPortal.Module.Util;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Xpo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;
using System.IO;
using System.Threading.Tasks;

namespace CashSwiftCashControlPortal.Module.Controllers
{
    public class DeviceViewController : ViewController
    {
        private IContainer components;
        private SimpleAction LockDeviceAction;
        private SimpleAction UnlockDeviceAction;
        private PopupWindowShowAction GenerateUptimeReportAction;

        public DeviceViewController() => InitializeComponent();

        protected override void OnActivated() => base.OnActivated();

        protected override void OnViewControlsCreated() => base.OnViewControlsCreated();

        protected override void OnDeactivated() => base.OnDeactivated();

        private void LockDevice_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            foreach (Device selectedObject in (IEnumerable)View.SelectedObjects)
                HandleLockDevice(selectedObject, true);
            ObjectSpace.CommitChanges();
            View.ObjectSpace.Refresh();
        }

        private void UnlockDeviceAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            foreach (Device selectedObject in (IEnumerable)View.SelectedObjects)
                HandleLockDevice(selectedObject, false);
            ObjectSpace.CommitChanges();
            View.ObjectSpace.Refresh();
        }

        private bool HandleLockDevice(Device device, bool lockDevice)
        {
            bool flag = false;
            if (device.enabled == lockDevice)
            {
                device.enabled = !lockDevice;
                DeviceLock deviceLock = new DeviceLock(((XPObjectSpace)ObjectSpace).Session)
                {
                    device_id = device,
                    locked = lockDevice,
                    locked_by_device = false,
                    lock_date = DateTime.Now,
                    locking_user =  null,
                    web_locking_user = SecuritySystem.CurrentUserName
                };
                if (device.enabled)
                {
                    device.login_attempts = 0;
                    device.login_cycles = 0;
                }
                ObjectSpace.SetModified(View.CurrentObject);
                flag = true;
            }
            return flag;
        }

        private void GenerateUptimeReportAction_CustomizePopupWindowParams(
          object sender,
          CustomizePopupWindowParamsEventArgs e)
        {
            CustomiseGenerateUptimeReportCheckerParams("Generate Uptime Report", e);
        }

        private void CustomiseGenerateUptimeReportCheckerParams(
          string caption,
          CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(GenerateUptimeReportPopupWindowParams));
            GenerateUptimeReportPopupWindowParams popupWindowParams = objectSpace.GetObject(new GenerateUptimeReportPopupWindowParams());
            if (popupWindowParams == null)
                return;
            DetailView detailView = Application.CreateDetailView(objectSpace, popupWindowParams);
            detailView.ViewEditMode = ViewEditMode.Edit;
            detailView.Caption = caption;
            e.View =  detailView;
        }

        private void GenerateUptimeReportAction_Execute(
          object sender,
          PopupWindowShowActionExecuteEventArgs e)
        {
            GenerateUptimeReport(e);
        }

        private void GenerateUptimeReport(PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                GenerateUptimeReportPopupWindowParams viewCurrentObject = e.PopupWindowViewCurrentObject as GenerateUptimeReportPopupWindowParams;
                Device selectedObject = (Device)View.SelectedObjects[0];
                Logger.Log.Info(SecuritySystem.CurrentUserName, GetType().Name, nameof(GenerateUptimeReport), "Generating Uptime Report for device '{0}'", selectedObject.ToString());
                ServerConfiguration serverConfiguration1 = ObjectSpace.FindObject<ServerConfiguration>(new BinaryOperator("config_key", "WEB_PORTAL_APP_ID"));
                ServerConfiguration serverConfiguration2 = ObjectSpace.FindObject<ServerConfiguration>(new BinaryOperator("config_key", "WEB_PORTAL_APP_KEY"));
                ServerConfiguration serverConfiguration3 = ObjectSpace.FindObject<ServerConfiguration>(new BinaryOperator("config_key", "CDM_REPORT_URL"));
                ServerConfiguration serverConfiguration4 = ObjectSpace.FindObject<ServerConfiguration>(new BinaryOperator("config_key", "CDM_REPORT_SAVE_PATH"));
                UptimeReportRequest uptimeReportRequest = new UptimeReportRequest
                {
                    AppID = new Guid(serverConfiguration1.config_value),
                    AppName = "Web Portal",
                    MessageDateTime = DateTime.Now,
                    MessageID = Guid.NewGuid().ToString(),
                    SessionID = Guid.NewGuid().ToString(),
                    Device = selectedObject.id,
                    FromDate = viewCurrentObject.StartDate.Date
                };
                DateTime dateTime = viewCurrentObject.EndDate;
                DateTime date1 = dateTime.Date;
                dateTime = viewCurrentObject.StartDate;
                DateTime min = dateTime.AddDays(1.0);
                dateTime = viewCurrentObject.EndDate;
                DateTime date2 = dateTime.Date;
                uptimeReportRequest.ToDate = date1.Clamp(min, date2);
                uptimeReportRequest.Language = "en-gb";
                uptimeReportRequest.UptimeReportPathFormat = viewCurrentObject.UptimeReportPathFormat;
                uptimeReportRequest.UptimeReportTempatePath = viewCurrentObject.UptimeReportTempatePath;
                UptimeReportRequest request = uptimeReportRequest;
                DateTime now = DateTime.Now;
                UptimeReportClient client = new UptimeReportClient(string.IsNullOrWhiteSpace(viewCurrentObject.CDM_URL) ? serverConfiguration3.config_value.Replace("machine_name", selectedObject.machine_name) : viewCurrentObject.CDM_URL, new Guid(serverConfiguration1.config_value), Convert.FromBase64String(serverConfiguration2.config_value), null);
                UptimeReportResponse result = Task.Run(() => client.GetUptimeReportAsync(request)).Result;
                string path = string.Format(string.IsNullOrWhiteSpace(viewCurrentObject.ReportSavePath) ? serverConfiguration4.config_value ?? "c:\\Server\\Reports\\UptimeReport\\{0}\\{1}" : viewCurrentObject.ReportSavePath, selectedObject.machine_name, result.FileName);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllBytes(path, result.ReportBytes);
            }
            catch (TimeoutException ex)
            {
                Logger.Log.Error(SecuritySystem.CurrentUserName, nameof(DeviceViewController), "GenerateUptimeReportAction_Execute", ex.MessageString());
                throw new Exception("Request timed out", ex);
            }
            catch (Exception ex)
            {
                Logger.Log.Error(SecuritySystem.CurrentUserName, nameof(DeviceViewController), "GenerateUptimeReportAction_Execute", ex.MessageString());
                throw new Exception("Server error. Contact administrator.", ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components =  new Container();
            LockDeviceAction = new SimpleAction(components);
            UnlockDeviceAction = new SimpleAction(components);
            GenerateUptimeReportAction = new PopupWindowShowAction(components);
            LockDeviceAction.ActionMeaning = ActionMeaning.Accept;
            LockDeviceAction.Caption = "Lock";
            LockDeviceAction.Category = "RecordEdit";
            LockDeviceAction.ConfirmationMessage = "Lock the device(s)?";
            LockDeviceAction.Id = "63e899cd-32a9-4fd7-8cdd-d1c6f7de1c18";
            LockDeviceAction.ImageName = "locked";
            LockDeviceAction.QuickAccess = true;
            LockDeviceAction.TargetObjectsCriteria = "enabled=true && UserHasActivityPermission('DEVICE_LOCK')";
            LockDeviceAction.TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueForAll;
            LockDeviceAction.TargetObjectType = typeof(Device);
            LockDeviceAction.ToolTip = "Lock device(s). Locked devices cannot conduct transactions.";
            LockDeviceAction.Execute += new SimpleActionExecuteEventHandler(LockDevice_Execute);
            UnlockDeviceAction.ActionMeaning = ActionMeaning.Accept;
            UnlockDeviceAction.Caption = "Unlock";
            UnlockDeviceAction.Category = "RecordEdit";
            UnlockDeviceAction.ConfirmationMessage = "Unlock device(s)?";
            UnlockDeviceAction.Id = "3abbac70-fbc7-45b0-be0e-c58b9f1ed264";
            UnlockDeviceAction.ImageName = "unlocked";
            UnlockDeviceAction.QuickAccess = true;
            UnlockDeviceAction.TargetObjectsCriteria = "enabled=false  && UserHasActivityPermission('DEVICE_UNLOCK')";
            UnlockDeviceAction.TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueForAll;
            UnlockDeviceAction.TargetObjectType = typeof(Device);
            UnlockDeviceAction.ToolTip = "Unlock Device(s). Once unlocked, a device can carry out transactions";
            UnlockDeviceAction.Execute += new SimpleActionExecuteEventHandler(UnlockDeviceAction_Execute);
            GenerateUptimeReportAction.AcceptButtonCaption = "Generate";
            GenerateUptimeReportAction.ActionMeaning = ActionMeaning.Accept;
            GenerateUptimeReportAction.Caption = "Uptime Report";
            GenerateUptimeReportAction.Category = "Reports";
            GenerateUptimeReportAction.ConfirmationMessage =  null;
            GenerateUptimeReportAction.Id = "a5cc1aa0-ed35-44f4-8c8a-214ab0dbd1bd";
            GenerateUptimeReportAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            GenerateUptimeReportAction.TargetObjectType = typeof(Device);
            GenerateUptimeReportAction.TargetViewNesting = Nesting.Root;
            GenerateUptimeReportAction.ToolTip =  null;
            GenerateUptimeReportAction.CustomizePopupWindowParams += new CustomizePopupWindowParamsEventHandler(GenerateUptimeReportAction_CustomizePopupWindowParams);
            GenerateUptimeReportAction.Execute += new PopupWindowShowActionExecuteEventHandler(GenerateUptimeReportAction_Execute);
            Actions.Add(LockDeviceAction);
            Actions.Add(UnlockDeviceAction);
            Actions.Add(GenerateUptimeReportAction);
        }
    }
}
