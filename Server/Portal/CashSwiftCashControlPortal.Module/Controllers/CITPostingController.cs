
//Controllers.CITPostingController


using CashSwift.Library.Standard.Utilities;
using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF;
using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF.Permissions;
using CashSwiftCashControlPortal.Module.BusinessObjects.CITs;
using CashSwiftCashControlPortal.Module.BusinessObjects.Transactions;
using CashSwiftCashControlPortal.Module.Util;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo.DB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;
using System.Linq;
using System.Text;

namespace CashSwiftCashControlPortal.Module.Controllers
{
    public class CITPostingController : ViewController
    {
        private IContainer components;
        private SimpleAction CITPostFailedTxAction;

        public CITPostingController() => InitializeComponent();

        protected override void OnActivated()
        {
            base.OnActivated();
            if (!(SecuritySystem.Instance is IRequestSecurity))
                return;
            CITPostFailedTxAction.Active.SetItemValue("Security", SecuritySystem.IsGranted(new MakerPermissionRequest(typeof(CITPosting))));
        }

        protected override void OnViewControlsCreated() => base.OnViewControlsCreated();

        protected override void OnDeactivated() => base.OnDeactivated();

        private void CITPostFailedTxAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            SecuritySystem.Demand(new MakerPermissionRequest(typeof(CITPosting)));
            ApplicationUser initialiser = ObjectSpace.GetObject(SecuritySystem.CurrentUser as ApplicationUser);
            foreach (CITTransaction selectedObject in (IEnumerable)View.SelectedObjects)
            {
                foreach (PostingProcCallResult postingProcCallResult in HandleCITPostInit(selectedObject, initialiser))
                {
                    if (string.IsNullOrWhiteSpace(postingProcCallResult.Error))
                    {
                        Logger.Log.Info(nameof(CITPostingController), "Processing", "CITPostingInsertResult", "CITTransaction [{0}] Success with CITPosting id: {1}", postingProcCallResult.ID, postingProcCallResult.ID);
                    }
                    else
                    {
                        string Message = string.Format("CITTransaction [{0}] Failed with error: {1}", postingProcCallResult.ID, postingProcCallResult.Error);
                        stringBuilder.AppendLine(string.Format("CITPosting CITTransaction [{0:0}] Failed", selectedObject.id));
                        Logger.Log.Warning(nameof(CITPostingController), "Processing", "CITPostingInsertResult", Message);
                    }
                }
            }
            string str = stringBuilder.ToString();
            if (!string.IsNullOrWhiteSpace(str))
                throw new UserFriendlyException(Environment.NewLine + str);
            ObjectSpace.CommitChanges();
            ObjectSpace.SetModified(View.CurrentObject);
            View.ObjectSpace.Refresh();
        }

        private IEnumerable<PostingProcCallResult> HandleCITPostInit(
          CITTransaction CITTransaction,
          ApplicationUser initialiser)
        {
            try
            {
                return PerformCITPostingInsertInDB(CITTransaction, initialiser);
            }
            catch (Exception ex)
            {
                Logger.Log.Error(nameof(CITPostingController), "Error", "PostingProcCallResult Error", ex.MessageString());
                throw new UserFriendlyException("Server Error. Contact Administrator");
            }
        }

        private IEnumerable<PostingProcCallResult> PerformCITPostingInsertInDB(
          CITTransaction CITTransaction,
          ApplicationUser initialiser)
        {
            return (ObjectSpace as XPObjectSpace).Session.ExecuteSprocParametrized("procInsCITPosting", new SprocParameter("@post_status", 2), new SprocParameter("@cit_tx_id", CITTransaction.id), new SprocParameter("@dr_account", CITTransaction.suspense_account), new SprocParameter("@dr_currency", CITTransaction.currency), new SprocParameter("@dr_amount", CITTransaction.amount), new SprocParameter("@cr_account", CITTransaction.account_number), new SprocParameter("@cr_currency", CITTransaction.currency), new SprocParameter("@cr_amount", CITTransaction.amount), new SprocParameter("@narration", CITTransaction.narration), new SprocParameter("@init_date", DateTime.Now), new SprocParameter("@initialising_user", initialiser.id), new SprocParameter("@device_initiated", 0)).ResultSet.SelectMany(x => x.Rows, (x, y) => new PostingProcCallResult()
            {
                ID = (Guid)y.Values[0],
                PostingID = (Guid?)y.Values[1],
                Error = (string)y.Values[2]
            });
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
            CITPostFailedTxAction = new SimpleAction(components)
            {
                ActionMeaning = ActionMeaning.Accept,
                Caption = "Post",
                Category = "ObjectsCreation",
                ConfirmationMessage = "This will post the selected CIT(s) to core banking. Are you sure?",
                Id = "C7C28DD1-89D9-4189-9087-832E24B626ED",
                PaintStyle = ActionItemPaintStyle.CaptionAndImage,
                TargetObjectsCriteria = "IsNullOrEmpty([cb_tx_number]) And Not [CITPostings][Not IsNullOrEmpty([cb_tx_number]) Or [is_complete] = False Or [auth_response] = '2'] And [TotalAmount] > 0.0",
                TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueForAll,
                TargetObjectType = typeof(CITTransaction),
                TargetViewId = "CITTransaction_ListView_FailedPost",
                TargetViewType = ViewType.ListView,
                ToolTip = "Post the selected CIT(s) to core banking",
                TypeOfView = typeof(ListView)
            };
            CITPostFailedTxAction.Execute += new SimpleActionExecuteEventHandler(CITPostFailedTxAction_Execute);
            Actions.Add(CITPostFailedTxAction);
            TargetObjectType = typeof(CITTransaction);
            TargetViewId = "CITTransaction_ListView_FailedPost";
            TargetViewType = ViewType.ListView;
            TypeOfView = typeof(ListView);
        }
    }
}
