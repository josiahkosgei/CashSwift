
//Controllers.DeviceStatusWindowController


using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Templates;
using System;

namespace CashSwiftCashControlPortal.Module.Controllers
{
    public class DeviceStatusWindowController : WindowController, IXafCallbackHandler
    {
        public DeviceStatusWindowController() => TargetWindowType = WindowType.Main;

        protected override void OnActivated()
        {
            base.OnActivated();
            Frame.ViewChanged += new EventHandler<ViewChangedEventArgs>(Frame_ViewChanged);
            ((WebWindow)Window).PagePreRender += new EventHandler(CurrentRequestWindow_PagePreRender);
        }

        protected override void OnDeactivated()
        {
            ((WebWindow)Window).PagePreRender -= new EventHandler(CurrentRequestWindow_PagePreRender);
            Frame.ViewChanged -= new EventHandler<ViewChangedEventArgs>(Frame_ViewChanged);
            base.OnDeactivated();
        }

        private void Frame_ViewChanged(object sender, ViewChangedEventArgs e)
        {
            if (Frame.View == null)
                return;
            Frame.View.ControlsCreated += new EventHandler(View_ControlsCreated);
        }

        private void View_ControlsCreated(object sender, EventArgs e) => RegisterXafCallackHandler();

        private void RegisterXafCallackHandler()
        {
            if (XafCallbackManager == null)
                return;
            XafCallbackManager.RegisterHandler("KA18958", this);
        }

        protected XafCallbackManager XafCallbackManager => WebWindow.CurrentRequestPage == null ? null : ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;

        private void CurrentRequestWindow_PagePreRender(object sender, EventArgs e)
        {
            WebWindow webWindow = (WebWindow)sender;
            string str = IsSuitableView() ? "window.startXafViewRefreshTimer(60000);" : "window.stopXafViewRefreshTimer();";
            string fullName = GetType().FullName;
            string script = str;
            webWindow.RegisterStartupScript(fullName, script);
        }

        public void ProcessAction(string parameter)
        {
            try
            {
                if (!IsSuitableView() || Frame.View == null)
                    return;
                Frame.View.ObjectSpace.Refresh();
            }
            catch (Exception ex)
            {
            }
        }

        protected virtual bool IsSuitableView() => Frame.View != null && Frame.View.IsRoot && !(Frame.View is DetailView) && !(Frame is NestedFrame) && !(Frame is PopupWindow) && Frame.View.Id == "DeviceStatus_ListView";
    }
}
