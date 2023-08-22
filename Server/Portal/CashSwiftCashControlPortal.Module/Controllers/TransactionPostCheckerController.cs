using CashSwift.API.Messaging.Integration;
using CashSwift.API.Messaging.Integration.Transactions;
using CashSwift.Library.Standard.Security;
using CashSwift.Library.Standard.Statuses;
using CashSwift.Library.Standard.Utilities;
using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF;
using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF.Permissions;
using CashSwiftCashControlPortal.Module.BusinessObjects.MakerChecker;
using CashSwiftCashControlPortal.Module.BusinessObjects.Server;
using CashSwiftCashControlPortal.Module.BusinessObjects.Transactions;
using CashSwiftCashControlPortal.Module.Properties;
using CashSwiftCashControlPortal.Module.Util;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo.DB;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;
using System.Linq;
using System.Threading.Tasks;

namespace CashSwiftCashControlPortal.Module.Controllers
{
    public class TransactionPostCheckerController : ObjectViewController
    {
        private double MessageKeepAliveTime = Settings.Default.MessageKeepAliveTime;
        private IContainer components;
        private PopupWindowShowAction AuthorisePostFailedTxAction;
        private PopupWindowShowAction RejectPostFailedTxAction;
        private PopupWindowShowAction BlockPostFailedAction;

        public TransactionPostCheckerController() => InitializeComponent();

        protected override void OnActivated()
        {
            base.OnActivated();
            if (!(SecuritySystem.Instance is IRequestSecurity))
                return;
            bool flag = SecuritySystem.IsGranted(new CheckerPermissionRequest(typeof(TransactionPosting)));
            AuthorisePostFailedTxAction.Active.SetItemValue("Security", flag);
            RejectPostFailedTxAction.Active.SetItemValue("Security", flag);
            BlockPostFailedAction.Active.SetItemValue("Security", flag);
        }

        protected override void OnViewControlsCreated() => base.OnViewControlsCreated();

        protected override void OnDeactivated() => base.OnDeactivated();

        private void CheckIntegrationResponseMessageDateTime(DateTime MessageDateTime)
        {
            DateTime dateTime1 = DateTime.Now.AddSeconds(MessageKeepAliveTime);
            DateTime dateTime2 = DateTime.Now.AddSeconds(-MessageKeepAliveTime);
            if (!(MessageDateTime < dateTime1) || !(MessageDateTime > dateTime2))
                throw new Exception(string.Format("Invalid MessageDateTime: value {0:yyyy-MM-dd HH:mm:ss.fff} is NOT between {1:yyyy-MM-dd HH:mm:ss.fff} and {2:yyyy-MM-dd HH:mm:ss.fff}", MessageDateTime, dateTime2, dateTime1));
        }

        private IEnumerable<PostingProcCallResult> HandleTransactionPostingUpdate(
          TransactionPosting TransactionPosting,
          ApplicationUser authoriser,
          MakerCheckerDecision decision,
          string reason)
        {
            try
            {
                return PerformTransactionPostingUpdateInDB(TransactionPosting, authoriser, decision, reason);
            }
            catch (Exception ex)
            {
                Logger.Log.Error("TransactionPostingController", "Error", "PostingProcCallResult Error", ex.MessageString());
                throw new UserFriendlyException("Server Error. Contact Administrator");
            }
        }

        private void AuthorisePostFailedTxAction_CustomizePopupWindowParams(
          object sender,
          CustomizePopupWindowParamsEventArgs e)
        {
            CustomiseAuthMakerCheckerParams("Authorise Posting?", e);
        }

        private void CustomiseAuthMakerCheckerParams(
          string caption,
          CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(MakerCheckerPopupWindowParams));
            MakerCheckerPopupWindowParams popupWindowParams = objectSpace.GetObject(new MakerCheckerPopupWindowParams());
            if (popupWindowParams == null)
                return;
            DetailView detailView = Application.CreateDetailView(objectSpace, popupWindowParams);
            detailView.ViewEditMode = ViewEditMode.Edit;
            detailView.Caption = caption;
            e.View =  detailView;
        }

        private void AuthorisePostFailedTxAction_Execute(
          object sender,
          PopupWindowShowActionExecuteEventArgs e)
        {
            HandleTransactionPostingAuth(MakerCheckerDecision.ACCEPT, e);
        }

        private void HandleTransactionPostingAuth(
          MakerCheckerDecision decision,
          PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                SecuritySystem.Demand(new CheckerPermissionRequest(typeof(TransactionPosting)));
                ApplicationUser authoriser = ObjectSpace.GetObject(SecuritySystem.CurrentUser as ApplicationUser);
                MakerCheckerPopupWindowParams viewCurrentObject = e.PopupWindowViewCurrentObject as MakerCheckerPopupWindowParams;
                foreach (TransactionPosting selectedObject in (IEnumerable)View.SelectedObjects)
                {
                    try
                    {
                        IEnumerable<PostingProcCallResult> postingProcCallResults = HandleTransactionPostingUpdate(selectedObject, authoriser, decision, viewCurrentObject.reason);
                        selectedObject.Session.Reload(selectedObject);
                        foreach (PostingProcCallResult postingProcCallResult in postingProcCallResults)
                        {
                            if (string.IsNullOrWhiteSpace(postingProcCallResult.Error))
                            {
                                Logger.Log.Info("TransactionPostingController", "Processing", "TransactionPostingUpdateResult", "Device Posting [{0}] Updated to {1}", postingProcCallResult.PostingID, decision);
                                if (decision == MakerCheckerDecision.ACCEPT)
                                {
                                    selectedObject.post_status = TransactionPostingStatus.POSTING;
                                    PostTransactionResponse coreBanking = PostToCoreBanking(selectedObject.tx_id.session_id.id, selectedObject.tx_id);
                                    selectedObject.post_date = coreBanking.MessageDateTime;
                                    selectedObject.cb_date = coreBanking.TransactionDateTime;
                                    selectedObject.cb_tx_status = coreBanking.PostResponseCode;
                                    selectedObject.cb_status_detail = coreBanking.PostResponseMessage;
                                    selectedObject.cb_tx_number = coreBanking.TransactionID;
                                    selectedObject.error_code = coreBanking.ServerErrorCode;
                                    selectedObject.error_message = coreBanking.ServerErrorMessage;
                                    try
                                    {
                                        selectedObject.tx_id.cb_date = DateTime.Now;
                                        selectedObject.tx_id.cb_tx_status = selectedObject.cb_tx_status;
                                        selectedObject.tx_id.cb_status_detail = selectedObject.cb_status_detail;
                                        selectedObject.tx_id.cb_tx_number = selectedObject.cb_tx_number;
                                        int result;
                                        selectedObject.tx_id.tx_error_code = int.TryParse(selectedObject.error_code, out result) ? result : 500;
                                        selectedObject.tx_id.tx_error_message = selectedObject.error_message.Left(byte.MaxValue);
                                        Logger.Log.Info(nameof(TransactionPostCheckerController), "Updating Transaction", nameof(HandleTransactionPostingAuth), "SUCCESS FT = {0}", selectedObject.tx_id.cb_tx_number);
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Log.Error(nameof(TransactionPostCheckerController), "Updating Transaction", nameof(HandleTransactionPostingAuth), "FAILED {0}", ex.MessageString());
                                    }
                                }
                                selectedObject.is_complete = true;
                                selectedObject.post_status = TransactionPostingStatus.COMPLETE;
                            }
                            else
                            {
                                string Message = string.Format("Device Posting [{0}] Failed with error: {1}", postingProcCallResult.PostingID, postingProcCallResult.Error);
                                Logger.Log.Warning("TransactionPostingController", "Processing", "TransactionPostingUpdateResult", Message);
                                selectedObject.error_code = "1";
                                selectedObject.error_message = postingProcCallResult.Error.Left(byte.MaxValue);
                                selectedObject.is_complete = true;
                                selectedObject.post_status = TransactionPostingStatus.ERROR;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        selectedObject.error_code = "1";
                        selectedObject.error_message = ex.MessageString(new int?(byte.MaxValue));
                        selectedObject.is_complete = true;
                        selectedObject.post_status = TransactionPostingStatus.ERROR;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error("TransactionPostingController", "POST ERROR", nameof(HandleTransactionPostingAuth), ex.MessageString());
            }
            ObjectSpace.CommitChanges();
            ObjectSpace.SetModified(View.CurrentObject);
            View.ObjectSpace.Refresh();
        }

        private void RejectPostFailedTxAction_CustomizePopupWindowParams(
          object sender,
          CustomizePopupWindowParamsEventArgs e)
        {
            CustomiseAuthMakerCheckerParams("Reject Posting?", e);
        }

        private void RejectPostFailedTxAction_Execute(
          object sender,
          PopupWindowShowActionExecuteEventArgs e)
        {
            HandleTransactionPostingAuth(MakerCheckerDecision.REJECT, e);
        }

        private IEnumerable<PostingProcCallResult> PerformTransactionPostingUpdateInDB(
          TransactionPosting TransactionPosting,
          ApplicationUser authoriser,
          MakerCheckerDecision decision,
          string reason)
        {
            return (ObjectSpace as XPObjectSpace).Session.ExecuteSprocParametrized("procUpdTransactionPostingAuth", new SprocParameter("@id", TransactionPosting.id), new SprocParameter("@post_status", 3), new SprocParameter("@tx_id", TransactionPosting.tx_id.id), new SprocParameter("@auth_date", DateTime.Now), new SprocParameter("@authorising_user", authoriser.id), new SprocParameter("@auth_response", decision), new SprocParameter("@reason", reason)).ResultSet.SelectMany(x => x.Rows, (x, y) => new PostingProcCallResult()
            {
                ID = (Guid)y.Values[0],
                PostingID = (Guid?)y.Values[1],
                Error = (string)y.Values[2]
            });
        }

        public PostTransactionResponse PostToCoreBanking(
          Guid requestID,
          Transaction transaction)
        {
            if (false)
            {
                Logger.Log.Info(SecuritySystem.CurrentUserName, GetType().Name, nameof(PostToCoreBanking), "Commands", "DebugPosting: RequestID = {0}, AccountNumber = {1}, Currency = {2}, Amount = {3:N2}", requestID, transaction.tx_account_number, transaction.tx_currency, transaction.TotalAmount);
                PostTransactionResponse coreBanking = new PostTransactionResponse
                {
                    MessageID = Guid.NewGuid().ToString().ToUpper(),
                    RequestID = Guid.NewGuid().ToString().ToUpper(),
                    PostResponseCode = string.Concat(200),
                    PostResponseMessage = "Posted",
                    MessageDateTime = DateTime.Now,
                    IsSuccess = true,
                    TransactionID = "FT" + PasswordGenerator.GenerateToken(6L, "1234567890")
                };
                return coreBanking;
            }
            try
            {
                Logger.Log.Info(SecuritySystem.CurrentUserName, GetType().Name, "Posting to live core banking", "Integation", "posting transaction {0}", transaction.ToString());
                Logger.Log.Info(SecuritySystem.CurrentUserName, GetType().Name, nameof(PostToCoreBanking), "Commands", "RequestID = {0}, AccountNumber = {1}, Suspense Account {4}, Currency = {2}, Amount = {3:N2}", requestID, transaction.tx_account_number, transaction.tx_currency, transaction.TotalAmount, transaction.tx_suspense_account);
                ServerConfiguration serverConfiguration1 = ObjectSpace.FindObject<ServerConfiguration>(new BinaryOperator("config_key", "WEB_PORTAL_APP_ID"));
                ServerConfiguration serverConfiguration2 = ObjectSpace.FindObject<ServerConfiguration>(new BinaryOperator("config_key", "WEB_PORTAL_APP_KEY"));
                ServerConfiguration serverConfiguration3 = ObjectSpace.FindObject<ServerConfiguration>(new BinaryOperator("config_key", "INTEGRATION_URL"));
                PostTransactionRequest transactionRequest1 = new PostTransactionRequest
                {
                    AppID = new Guid(serverConfiguration1.config_value),
                    AppName = "Web Portal",
                    MessageDateTime = DateTime.Now
                };
                Guid guid = Guid.NewGuid();
                transactionRequest1.MessageID = guid.ToString();
                guid = Guid.NewGuid();
                transactionRequest1.SessionID = guid.ToString();
                transactionRequest1.DeviceID = transaction.device_id.id;
                transactionRequest1.FundsSource = transaction.funds_source;
                transactionRequest1.DepositorIDNumber = transaction.tx_id_number;
                transactionRequest1.DepositorName = transaction.tx_depositor_name;
                transactionRequest1.DepositorPhone = transaction.tx_phone;
                transactionRequest1.RefAccountName = transaction.cb_ref_account_name;
                transactionRequest1.RefAccountNumber = transaction.tx_ref_account;
                transactionRequest1.TransactionType = transaction.tx_type.cb_tx_type;
                transactionRequest1.DeviceReferenceNumber = string.Format("{0:0}", transaction.tx_random_number);
                transactionRequest1.Transaction = new PostTransactionData()
                {
                    TransactionID = transaction.id,
                    CreditAccount = new PostBankAccount()
                    {
                        AccountName = transaction.cb_account_name,
                        AccountNumber = transaction.tx_account_number,
                        Currency = transaction.tx_currency.code.ToUpperInvariant()
                    },
                    DebitAccount = new PostBankAccount()
                    {
                        AccountNumber = transaction.tx_suspense_account,
                        Currency = transaction.tx_currency.code.ToUpperInvariant()
                    },
                    Amount = transaction.TotalAmount,
                    DateTime = DateTime.Now,
                    DeviceID = transaction.device_id.id,
                    DeviceNumber = transaction.device_id.device_number,
                    Narration = transaction.tx_narration
                };
                PostTransactionRequest transactionRequest = transactionRequest1;
                DateTime now = DateTime.Now;
                IntegrationServiceClient CoreBankingClient = new IntegrationServiceClient(serverConfiguration3.config_value, new Guid(serverConfiguration1.config_value), Convert.FromBase64String(serverConfiguration2.config_value), null);
                PostTransactionResponse result = Task.Run(() => CoreBankingClient.PostTransactionAsync(transactionRequest)).Result;
                CheckIntegrationResponseMessageDateTime(result.MessageDateTime);
                return result;
            }
            catch (Exception ex)
            {
                string str = string.Format("Post failed with error: {0}>>{1}>>{2}", ex.Message, ex.InnerException?.Message, ex.InnerException?.InnerException?.Message);
                Logger.Log.Error(SecuritySystem.CurrentUserName, GetType().Name, nameof(PostToCoreBanking), ApplicationErrorConst.ERROR_TRANSACTION_POST_FAILURE.ToString(), str);
                PostTransactionResponse coreBanking = new PostTransactionResponse
                {
                    MessageDateTime = DateTime.Now,
                    PostResponseCode = "-1",
                    PostResponseMessage = str,
                    RequestID = requestID.ToString().ToUpperInvariant(),
                    ServerErrorCode = "-1",
                    ServerErrorMessage = str,
                    IsSuccess = false
                };
                return coreBanking;
            }
        }

        private void BlockPostFailedAction_Execute(
          object sender,
          PopupWindowShowActionExecuteEventArgs e)
        {
            HandleTransactionPostingAuth(MakerCheckerDecision.DISALLOW, e);
        }

        private void BlockPostFailedAction_CustomizePopupWindowParams(
          object sender,
          CustomizePopupWindowParamsEventArgs e)
        {
            CustomiseAuthMakerCheckerParams("Permanently Block Posting?", e);
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
            AuthorisePostFailedTxAction = new PopupWindowShowAction(components);
            RejectPostFailedTxAction = new PopupWindowShowAction(components);
            BlockPostFailedAction = new PopupWindowShowAction(components);
            AuthorisePostFailedTxAction.AcceptButtonCaption = "Yes";
            AuthorisePostFailedTxAction.CancelButtonCaption = "No";
            AuthorisePostFailedTxAction.Caption = "Authorise";
            AuthorisePostFailedTxAction.ConfirmationMessage = "The transaction(s) will be posted to core banking. Continue?";
            AuthorisePostFailedTxAction.Id = "b191f358-e5e9-42da-bd56-dfc972ab1cc0";
            AuthorisePostFailedTxAction.PaintStyle = ActionItemPaintStyle.CaptionAndImage;
            AuthorisePostFailedTxAction.TargetObjectsCriteria = "[initialising_user].[id]!=CurrentUserId() && [is_complete] = False";
            AuthorisePostFailedTxAction.TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueForAll;
            AuthorisePostFailedTxAction.TargetObjectType = typeof(TransactionPosting);
            AuthorisePostFailedTxAction.TargetViewId = "TransactionPosting_ListView_Pending";
            AuthorisePostFailedTxAction.ToolTip = "The transaction(s) will be posted to core banking.";
            AuthorisePostFailedTxAction.CustomizePopupWindowParams += new CustomizePopupWindowParamsEventHandler(AuthorisePostFailedTxAction_CustomizePopupWindowParams);
            AuthorisePostFailedTxAction.Execute += new PopupWindowShowActionExecuteEventHandler(AuthorisePostFailedTxAction_Execute);
            RejectPostFailedTxAction.AcceptButtonCaption = "Yes";
            RejectPostFailedTxAction.CancelButtonCaption = "No";
            RejectPostFailedTxAction.Caption = "Reject";
            RejectPostFailedTxAction.ConfirmationMessage = "Reject the transaction(s) from posting to core banking. Continue?";
            RejectPostFailedTxAction.Id = "5eb1c4d4-a0c1-48bb-bb53-52eebbbaf24a";
            RejectPostFailedTxAction.PaintStyle = ActionItemPaintStyle.CaptionAndImage;
            RejectPostFailedTxAction.TargetObjectsCriteria = "[initialising_user].[id]!=CurrentUserId() && [is_complete] = False";
            RejectPostFailedTxAction.TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueForAll;
            RejectPostFailedTxAction.TargetObjectType = typeof(TransactionPosting);
            RejectPostFailedTxAction.TargetViewId = "TransactionPosting_ListView_Pending";
            RejectPostFailedTxAction.ToolTip = "Reject the transaction(s) from posting to core banking. ";
            RejectPostFailedTxAction.CustomizePopupWindowParams += new CustomizePopupWindowParamsEventHandler(RejectPostFailedTxAction_CustomizePopupWindowParams);
            RejectPostFailedTxAction.Execute += new PopupWindowShowActionExecuteEventHandler(RejectPostFailedTxAction_Execute);
            BlockPostFailedAction.AcceptButtonCaption = "Yes";
            BlockPostFailedAction.CancelButtonCaption = "No";
            BlockPostFailedAction.Caption = "Block!";
            BlockPostFailedAction.ConfirmationMessage = "Block the transaction(s) from posting to core banking. Blocked transaction will never be allowed to post through the Web Portal. Continue?";
            BlockPostFailedAction.Id = "8548408B-9332-4D1A-A50D-23EF06F54AAE";
            BlockPostFailedAction.PaintStyle = ActionItemPaintStyle.CaptionAndImage;
            BlockPostFailedAction.TargetObjectsCriteria = "[initialising_user].[id]!=CurrentUserId() && [is_complete] = False";
            BlockPostFailedAction.TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueForAll;
            BlockPostFailedAction.TargetObjectType = typeof(TransactionPosting);
            BlockPostFailedAction.TargetViewId = "TransactionPosting_ListView_Pending";
            BlockPostFailedAction.ToolTip = "Block the transaction(s) from posting to core banking. Blocked transaction will never be allowed to post through the Web Portal.";
            BlockPostFailedAction.CustomizePopupWindowParams += new CustomizePopupWindowParamsEventHandler(BlockPostFailedAction_CustomizePopupWindowParams);
            BlockPostFailedAction.Execute += new PopupWindowShowActionExecuteEventHandler(BlockPostFailedAction_Execute);
            Actions.Add(AuthorisePostFailedTxAction);
            Actions.Add(RejectPostFailedTxAction);
            Actions.Add(BlockPostFailedAction);
            TargetObjectType = typeof(TransactionPosting);
            TargetViewId = "TransactionPosting_ListView_Pending";
            TargetViewType = ViewType.ListView;
            TypeOfView = typeof(ListView);
        }
    }
}
