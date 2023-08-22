
//Controllers.TransactionPostingController


using CashSwift.Library.Standard.Utilities;
using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF;
using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF.Permissions;
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
    public class TransactionPostingController : ObjectViewController
    {
        private IContainer components;
        private SimpleAction PostFailedTxAction;

        public TransactionPostingController() => InitializeComponent();

        protected override void OnActivated()
        {
            base.OnActivated();
            if (!(SecuritySystem.Instance is IRequestSecurity))
                return;
            PostFailedTxAction.Active.SetItemValue("Security", SecuritySystem.IsGranted(new MakerPermissionRequest(typeof(TransactionPosting))));
        }

        protected override void OnViewControlsCreated() => base.OnViewControlsCreated();

        protected override void OnDeactivated() => base.OnDeactivated();

        private void PostFailedTxAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            SecuritySystem.Demand(new MakerPermissionRequest(typeof(TransactionPosting)));
            ApplicationUser initialiser = ObjectSpace.GetObject(SecuritySystem.CurrentUser as ApplicationUser);
            foreach (Transaction selectedObject in (IEnumerable)View.SelectedObjects)
            {
                foreach (PostingProcCallResult postingProcCallResult in HandlePostInit(selectedObject, initialiser))
                {
                    if (string.IsNullOrWhiteSpace(postingProcCallResult.Error))
                    {
                        Logger.Log.Info(nameof(TransactionPostingController), "Processing", "TransactionPostingInsertResult", "Transaction [{0}] Success with TransactionPosting id: {1}", postingProcCallResult.ID, postingProcCallResult.PostingID);
                    }
                    else
                    {
                        string Message = string.Format("Transaction [{0}] Failed with error: {1}", postingProcCallResult.ID, postingProcCallResult.Error);
                        stringBuilder.AppendLine(string.Format("Posting Transaction [{0:0}] Failed", selectedObject.tx_random_number));
                        Logger.Log.Warning(nameof(TransactionPostingController), "Processing", "TransactionPostingInsertResult", Message);
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

        private IEnumerable<PostingProcCallResult> HandlePostInit(
          Transaction transaction,
          ApplicationUser initialiser)
        {
            try
            {
                return PerformTransactionPostingInsertInDB(transaction, initialiser);
            }
            catch (Exception ex)
            {
                Logger.Log.Error(nameof(TransactionPostingController), "Error", "PostingProcCallResult Error", ex.MessageString());
                throw new UserFriendlyException("Server Error. Contact Administrator");
            }
        }

        private IEnumerable<PostingProcCallResult> PerformTransactionPostingInsertInDB(
          Transaction transaction,
          ApplicationUser initialiser)
        {
            return (ObjectSpace as XPObjectSpace).Session.ExecuteSprocParametrized("procInsTransactionPosting", new SprocParameter("@post_status", 2), new SprocParameter("@tx_id", transaction.id), new SprocParameter("@dr_account", transaction.tx_suspense_account), new SprocParameter("@dr_currency", transaction.tx_currency.code), new SprocParameter("@dr_amount", transaction.tx_amount), new SprocParameter("@cr_account", transaction.tx_account_number), new SprocParameter("@cr_currency", transaction.tx_currency.code), new SprocParameter("@cr_amount", transaction.tx_amount), new SprocParameter("@narration", transaction.tx_narration), new SprocParameter("@init_date", DateTime.Now), new SprocParameter("@initialising_user", initialiser.id), new SprocParameter("@device_initiated", 0)).ResultSet.SelectMany(x => x.Rows, (x, y) => new PostingProcCallResult()
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
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(TransactionPostingController));
            PostFailedTxAction = new SimpleAction(components)
            {
                ActionMeaning = ActionMeaning.Accept,
                Caption = "Post",
                Category = "ObjectsCreation",
                ConfirmationMessage = "This will post the selected transaction(s) to core banking. Are you sure?",
                Id = "c5dcd632-19a6-45a5-8f8d-27585bb15492",
                PaintStyle = ActionItemPaintStyle.CaptionAndImage,
                TargetObjectsCriteria = componentResourceManager.GetString("PostFailedTxAction.TargetObjectsCriteria"),
                TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueForAll,
                TargetObjectType = typeof(Transaction),
                TargetViewId = "Transaction_ListView_FailedPost",
                ToolTip = "Post the selected transaction(s) to core banking"
            };
            PostFailedTxAction.Execute += new SimpleActionExecuteEventHandler(PostFailedTxAction_Execute);
            Actions.Add(PostFailedTxAction);
            TargetObjectType = typeof(Transaction);
            TargetViewId = "Transaction_ListView_FailedPost";
            TargetViewType = ViewType.ListView;
            TypeOfView = typeof(ListView);
        }
    }
}
