
//CashControlPortalModule


using CashSwiftCashControlPortal.Module.CustomFunctionOperator;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.AuditTrail;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.TreeListEditors;
using DevExpress.ExpressApp.TreeListEditors.Web;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Validation;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module
{
    public sealed class CashControlPortalModule : ModuleBase
    {
        private IContainer components;

        public CashControlPortalModule()
        {
            InitializeComponent();
            BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
            IsAllowedByUserGroupOperator.Register();
            IsVisibleByUserGroupOperator.Register();
            UserIsAdministrativeOperator.Register();
            CurrentUserUsernameOperator.Register();
            UserHasActivityPermissionOperator.Register();
        }

        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(
          IObjectSpace objectSpace,
          Version versionFromDB)
        {
            return new ModuleUpdater[1]
            {
         new DatabaseUpdate.Updater(objectSpace, versionFromDB)
            };
        }

        public override void Setup(XafApplication application) => base.Setup(application);

        public override void CustomizeTypesInfo(ITypesInfo typesInfo)
        {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            AdditionalExportedTypes.Add(typeof(ModelDifference));
            AdditionalExportedTypes.Add(typeof(ModelDifferenceAspect));
            AdditionalExportedTypes.Add(typeof(PermissionPolicyRoleBase));
            AdditionalExportedTypes.Add(typeof(BaseObject));
            AdditionalExportedTypes.Add(typeof(PermissionPolicyTypePermissionObject));
            AdditionalExportedTypes.Add(typeof(PermissionPolicyNavigationPermissionObject));
            AdditionalExportedTypes.Add(typeof(XPCustomObject));
            AdditionalExportedTypes.Add(typeof(XPBaseObject));
            AdditionalExportedTypes.Add(typeof(PersistentBase));
            AdditionalExportedTypes.Add(typeof(PermissionPolicyMemberPermissionsObject));
            AdditionalExportedTypes.Add(typeof(PermissionPolicyObjectPermissionsObject));
            RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.SystemModule.SystemModule));
            RequiredModuleTypes.Add(typeof(ValidationModule));
            RequiredModuleTypes.Add(typeof(ConditionalAppearanceModule));
            RequiredModuleTypes.Add(typeof(TreeListEditorsAspNetModule));
            RequiredModuleTypes.Add(typeof(TreeListEditorsModuleBase));
            RequiredModuleTypes.Add(typeof(AuditTrailModule));
        }
    }
}
