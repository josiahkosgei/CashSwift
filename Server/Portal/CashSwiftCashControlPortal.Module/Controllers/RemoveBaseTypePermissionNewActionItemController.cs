
//Controllers.RemoveBaseTypePermissionNewActionItemController


using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF.Permissions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.Controllers
{
    public class RemoveBaseTypePermissionNewActionItemController :
      ObjectViewController<ObjectView, PermissionPolicyTypePermissionObject>
    {
        private IContainer components;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent() => components =  new Container();

        protected override void OnFrameAssigned()
        {
            NewObjectViewController controller = Frame.GetController<NewObjectViewController>();
            if (controller != null)
            {
                controller.CollectDescendantTypes +=  (s, e) => e.Types.Remove(typeof(PermissionPolicyTypePermissionObject));
                controller.ObjectCreating +=  (s, e) =>
                {
                    if (!(e.ObjectType == typeof(PermissionPolicyTypePermissionObject)))
                        return;
                    e.NewObject = e.ObjectSpace.CreateObject(typeof(MakerTypePermissionObject));
                };
            }
            base.OnFrameAssigned();
        }

        public RemoveBaseTypePermissionNewActionItemController() => InitializeComponent();

        protected override void OnActivated() => base.OnActivated();

        protected override void OnViewControlsCreated() => base.OnViewControlsCreated();

        protected override void OnDeactivated() => base.OnDeactivated();
    }
}
