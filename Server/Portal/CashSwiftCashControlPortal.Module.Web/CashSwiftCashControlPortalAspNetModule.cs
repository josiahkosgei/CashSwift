namespace CashSwiftCashControlPortal.Module.Web
{
    using CashSwiftCashControlPortal.Module;
    using DevExpress.ExpressApp;
    using DevExpress.ExpressApp.HtmlPropertyEditor.Web;
    using DevExpress.ExpressApp.Updating;
    using DevExpress.ExpressApp.Validation.Web;
    using DevExpress.ExpressApp.Web.SystemModule;
    using DevExpress.Persistent.BaseImpl;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [ToolboxItemFilter("Xaf.Platform.Web")]
    public sealed class CashControlPortalAspNetModule : ModuleBase
    {
        private IContainer components;

        public CashControlPortalAspNetModule()
        {
            this.InitializeComponent();
        }

        private void Application_CreateCustomUserModelDifferenceStore(object sender, CreateCustomModelDifferenceStoreEventArgs e)
        {
            e.Store = new ModelDifferenceDbStore((XafApplication)sender, typeof(ModelDifference), false, "Web");
            e.Handled = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) =>
            ModuleUpdater.EmptyModuleUpdaters;

        private void InitializeComponent()
        {
            base.RequiredModuleTypes.Add(typeof(CashControlPortalModule));
            base.RequiredModuleTypes.Add(typeof(SystemAspNetModule));
            base.RequiredModuleTypes.Add(typeof(HtmlPropertyEditorAspNetModule));
            base.RequiredModuleTypes.Add(typeof(ValidationAspNetModule));
        }

        public override void Setup(XafApplication application)
        {
            base.Setup(application);
            application.CreateCustomUserModelDifferenceStore += new EventHandler<CreateCustomModelDifferenceStoreEventArgs>(this.Application_CreateCustomUserModelDifferenceStore);
        }
    }
}

