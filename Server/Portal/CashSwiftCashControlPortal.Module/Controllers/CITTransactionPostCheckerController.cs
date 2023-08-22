using CashSwift.API.Messaging.Integration;
using CashSwift.API.Messaging.Integration.Transactions;
using CashSwift.Library.Standard.Security;
using CashSwift.Library.Standard.Statuses;
using CashSwift.Library.Standard.Utilities;
using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF;
using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF.Permissions;
using CashSwiftCashControlPortal.Module.BusinessObjects.CITs;
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
using System.Linq;
using System.Threading.Tasks;

namespace CashSwiftCashControlPortal.Module.Controllers
{
    public class CITTransactionPostCheckerController : ViewController
    {
        private double MessageKeepAliveTime = Settings.Default.MessageKeepAliveTime;
        private IContainer components;
        private PopupWindowShowAction AuthoriseCITPostFailedTxAction;
        private PopupWindowShowAction RejectCITPostFailedTxAction;
        private PopupWindowShowAction BlockCITPostFailedAction;

        public CITTransactionPostCheckerController() => InitializeComponent();

        protected override void OnActivated()
        {
            base.OnActivated();
            if (!(SecuritySystem.Instance is IRequestSecurity))
                return;
            bool flag = SecuritySystem.IsGranted(new CheckerPermissionRequest(typeof(CITPosting)));
            AuthoriseCITPostFailedTxAction.Active.SetItemValue("Security", flag);
            RejectCITPostFailedTxAction.Active.SetItemValue("Security", flag);
            BlockCITPostFailedAction.Active.SetItemValue("Security", flag);
        }

        protected override void OnViewControlsCreated() => base.OnViewControlsCreated();

        protected override void OnDeactivated() => base.OnDeactivated();

        private IEnumerable<PostingProcCallResult> HandleCITPostingUpdate(
          CITPosting CITPosting,
          ApplicationUser authoriser,
          MakerCheckerDecision decision,
          string reason)
        {
            try
            {
                return PerformCITPostingUpdateInDB(CITPosting, authoriser, decision, reason);
            }
            catch (Exception ex)
            {
                Logger.Log.Error("CITPostingController", "Error", "PostingProcCallResult Error", ex.MessageString());
                throw new UserFriendlyException("Server Error. Contact Administrator");
            }
        }

        private void AuthoriseCITPostFailedTxAction_CustomizePopupWindowParams(
          object sender,
          CustomizePopupWindowParamsEventArgs e)
        {
            CustomiseAuthMakerCheckerParams("Authorise CITPosting?", e);
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

        private void AuthoriseCITPostFailedTxAction_Execute(
          object sender,
          PopupWindowShowActionExecuteEventArgs e)
        {
            HandleCITPostingAuth(MakerCheckerDecision.ACCEPT, e);
        }

        private void HandleCITPostingAuth(
          MakerCheckerDecision decision,
          PopupWindowShowActionExecuteEventArgs e)
        {
            SecuritySystem.Demand(new CheckerPermissionRequest(typeof(CITPosting)));
            ApplicationUser authoriser = ObjectSpace.GetObject(SecuritySystem.CurrentUser as ApplicationUser);
            MakerCheckerPopupWindowParams viewCurrentObject = e.PopupWindowViewCurrentObject as MakerCheckerPopupWindowParams;
            foreach (CITPosting selectedObject in (IEnumerable)View.SelectedObjects)
            {
                try
                {
                    IEnumerable<PostingProcCallResult> postingProcCallResults = HandleCITPostingUpdate(selectedObject, authoriser, decision, viewCurrentObject.reason);
                    selectedObject.Session.Reload(selectedObject);
                    foreach (PostingProcCallResult postingProcCallResult in postingProcCallResults)
                    {
                        if (string.IsNullOrWhiteSpace(postingProcCallResult.Error))
                        {
                            Logger.Log.Info("CITPostingController", "Processing", "CITPostingUpdateResult", "Device CITPosting [{0}] Updated to {1}", postingProcCallResult.PostingID, decision);
                            if (decision == MakerCheckerDecision.ACCEPT)
                            {
                                selectedObject.post_status = CITPostingStatus.POSTING;
                                PostCITTransactionResponse coreBanking = CITPostToCoreBanking(selectedObject.cit_tx_id.cit_id.device_id.id, selectedObject.cit_tx_id);
                                selectedObject.post_date = coreBanking.MessageDateTime;
                                selectedObject.cb_date = coreBanking.TransactionDateTime;
                                selectedObject.cb_tx_status = coreBanking.PostResponseCode;
                                selectedObject.cb_status_detail = coreBanking.PostResponseMessage;
                                selectedObject.cb_tx_number = coreBanking.TransactionID;
                                selectedObject.error_code = coreBanking.ServerErrorCode;
                                selectedObject.error_message = coreBanking.ServerErrorMessage;
                                try
                                {
                                    selectedObject.cit_tx_id.cb_date = DateTime.Now;
                                    selectedObject.cit_tx_id.cb_tx_status = selectedObject.cb_tx_status;
                                    selectedObject.cit_tx_id.cb_status_detail = selectedObject.cb_status_detail;
                                    selectedObject.cit_tx_id.cb_tx_number = selectedObject.cb_tx_number;
                                    int result;
                                    selectedObject.cit_tx_id.error_code = int.TryParse(selectedObject.error_code, out result) ? result : 500;
                                    selectedObject.cit_tx_id.error_message = selectedObject.error_message.Left(byte.MaxValue);
                                    Logger.Log.Info(nameof(CITTransactionPostCheckerController), "Updating CITTransaction", nameof(HandleCITPostingAuth), "SUCCESS FT = {0}", selectedObject.cit_tx_id.cb_tx_number);
                                }
                                catch (Exception ex)
                                {
                                    Logger.Log.Error(nameof(CITTransactionPostCheckerController), "Updating CITTransaction", nameof(HandleCITPostingAuth), "FAILED {0}", ex.MessageString());
                                }
                            }
                            selectedObject.is_complete = true;
                            selectedObject.post_status = CITPostingStatus.COMPLETE;
                        }
                        else
                        {
                            string Message = string.Format("Device CITPosting [{0}] Failed with error: {1}", postingProcCallResult.PostingID, postingProcCallResult.Error);
                            Logger.Log.Warning("CITPostingController", "Processing", "CITPostingUpdateResult", Message);
                            selectedObject.error_code = "1";
                            selectedObject.error_message = postingProcCallResult.Error.Left(byte.MaxValue);
                            selectedObject.is_complete = true;
                            selectedObject.post_status = CITPostingStatus.ERROR;
                        }
                    }
                }
                catch (Exception ex)
                {
                    selectedObject.error_code = "1";
                    selectedObject.error_message = ex.MessageString(new int?(byte.MaxValue));
                    selectedObject.is_complete = true;
                    selectedObject.post_status = CITPostingStatus.ERROR;
                }
            }
            ObjectSpace.CommitChanges();
            ObjectSpace.SetModified(View.CurrentObject);
            View.ObjectSpace.Refresh();
        }

        private void RejectCITPostFailedTxAction_CustomizePopupWindowParams(
          object sender,
          CustomizePopupWindowParamsEventArgs e)
        {
            CustomiseAuthMakerCheckerParams("Reject CITPosting?", e);
        }

        private void RejectCITPostFailedTxAction_Execute(
          object sender,
          PopupWindowShowActionExecuteEventArgs e)
        {
            HandleCITPostingAuth(MakerCheckerDecision.REJECT, e);
        }

        private IEnumerable<PostingProcCallResult> PerformCITPostingUpdateInDB(
          CITPosting CITPosting,
          ApplicationUser authoriser,
          MakerCheckerDecision decision,
          string reason)
        {
            return (ObjectSpace as XPObjectSpace).Session.ExecuteSprocParametrized("procUpdCITPostingAuth", new SprocParameter("@id", CITPosting.id), new SprocParameter("@post_status", 3), new SprocParameter("@cit_tx_id", CITPosting.cit_tx_id.id), new SprocParameter("@auth_date", DateTime.Now), new SprocParameter("@authorising_user", authoriser.id), new SprocParameter("@auth_response", decision), new SprocParameter("@reason", reason.Left(100))).ResultSet.SelectMany(x => x.Rows, (x, y) => new PostingProcCallResult()
            {
                ID = (Guid)y.Values[0],
                PostingID = (Guid?)y.Values[1],
                Error = (string)y.Values[2]
            });
        }

        public PostCITTransactionResponse CITPostToCoreBanking(
          Guid requestID,
          CITTransaction CITTransaction)
        {
            if (false)
            {
                Logger.Log.Info(GetType().Name, nameof(CITPostToCoreBanking), "Commands", "DebugCITPosting: RequestID = {0}, AccountNumber = {1}, Currency = {2}, Amount = {3:N2}", requestID, CITTransaction.account_number, CITTransaction.currency, CITTransaction.TotalAmount);
                PostCITTransactionResponse coreBanking = new PostCITTransactionResponse
                {
                    MessageID = Guid.NewGuid().ToString().ToUpper(),
                    RequestID = Guid.NewGuid().ToString().ToUpper(),
                    PostResponseCode = string.Concat(200),
                    PostResponseMessage = "CITPosted",
                    MessageDateTime = DateTime.Now,
                    IsSuccess = true,
                    TransactionID = "FT" + PasswordGenerator.GenerateToken(6L, "1234567890")
                };
                return coreBanking;
            }
            try
            {
                Logger.Log.Info(GetType().Name, "CITPosting to live core banking", "Integation", "CITPosting CITTransaction {0}", CITTransaction.ToString());
                Logger.Log.Info(GetType().Name, nameof(CITPostToCoreBanking), "Commands", "RequestID = {0}, AccountNumber = {1}, Suspense Account {4}, Currency = {2}, Amount = {3:N2}", requestID, CITTransaction.account_number, CITTransaction.currency, CITTransaction.TotalAmount, CITTransaction.suspense_account);
                ServerConfiguration serverConfiguration1 = ObjectSpace.FindObject<ServerConfiguration>(new BinaryOperator("config_key", "WEB_PORTAL_APP_ID"));
                ServerConfiguration serverConfiguration2 = ObjectSpace.FindObject<ServerConfiguration>(new BinaryOperator("config_key", "WEB_PORTAL_APP_KEY"));
                ServerConfiguration serverConfiguration3 = ObjectSpace.FindObject<ServerConfiguration>(new BinaryOperator("config_key", "INTEGRATION_URL"));
                PostCITTransactionRequest transactionRequest = new PostCITTransactionRequest
                {
                    AppID = new Guid(serverConfiguration1.config_value),
                    AppName = "Web Portal",
                    MessageDateTime = DateTime.Now,
                    MessageID = Guid.NewGuid().ToString(),
                    SessionID = Guid.NewGuid().ToString(),
                    DeviceID = CITTransaction.cit_id.device_id.id,
                    Language = "en-gb",
                    Transaction = new PostTransactionData()
                    {
                        TransactionID = CITTransaction.id,
                        DebitAccount = new PostBankAccount()
                        {
                            AccountNumber = CITTransaction.account_number,
                            Currency = CITTransaction.currency.ToUpperInvariant()
                        },
                        CreditAccount = new PostBankAccount()
                        {
                            AccountNumber = CITTransaction.suspense_account,
                            Currency = CITTransaction.currency.ToUpperInvariant()
                        },
                        Amount = CITTransaction.TotalAmount,
                        DateTime = DateTime.Now,
                        DeviceID = CITTransaction.cit_id.device_id.id,
                        DeviceNumber = CITTransaction.cit_id.device_id.device_number,
                        Narration = CITTransaction.narration
                    }
                };
                PostCITTransactionRequest CITRequest = transactionRequest;
                DateTime now = DateTime.Now;
                IntegrationServiceClient CoreBankingClient = new IntegrationServiceClient(serverConfiguration3.config_value, new Guid(serverConfiguration1.config_value), Convert.FromBase64String(serverConfiguration2.config_value), null);
                PostCITTransactionResponse result = Task.Run(() => CoreBankingClient.PostCITTransactionAsync(CITRequest)).Result;
                CheckIntegrationResponseMessageDateTime(result.MessageDateTime);
                return result;
            }
            catch (Exception ex)
            {
                string Message = "CITPost failed with error: " + ex.MessageString();
                Logger.Log.Error(GetType().Name, ApplicationErrorConst.ERROR_TRANSACTION_POST_FAILURE.ToString(), nameof(CITPostToCoreBanking), Message);
                PostCITTransactionResponse coreBanking = new PostCITTransactionResponse
                {
                    MessageDateTime = DateTime.Now,
                    PostResponseCode = "-1",
                    PostResponseMessage = Message,
                    RequestID = requestID.ToString().ToUpperInvariant(),
                    ServerErrorCode = "-1",
                    ServerErrorMessage = Message,
                    IsSuccess = false
                };
                return coreBanking;
            }
        }

        private void CheckIntegrationResponseMessageDateTime(DateTime MessageDateTime)
        {
            DateTime dateTime1 = DateTime.Now.AddSeconds(MessageKeepAliveTime);
            DateTime dateTime2 = DateTime.Now.AddSeconds(-MessageKeepAliveTime);
            if (!(MessageDateTime < dateTime1) || !(MessageDateTime > dateTime2))
                throw new Exception(string.Format("Invalid MessageDateTime: value {0:yyyy-MM-dd HH:mm:ss.fff} is NOT between {1:yyyy-MM-dd HH:mm:ss.fff} and {2:yyyy-MM-dd HH:mm:ss.fff}", MessageDateTime, dateTime2, dateTime1));
        }

        private void BlockCITPostFailedAction_Execute(
          object sender,
          PopupWindowShowActionExecuteEventArgs e)
        {
            HandleCITPostingAuth(MakerCheckerDecision.DISALLOW, e);
        }

        private void BlockCITPostFailedAction_CustomizePopupWindowParams(
          object sender,
          CustomizePopupWindowParamsEventArgs e)
        {
            CustomiseAuthMakerCheckerParams("Permanently Block CITPosting?", e);
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
            AuthoriseCITPostFailedTxAction = new PopupWindowShowAction(components);
            RejectCITPostFailedTxAction = new PopupWindowShowAction(components);
            BlockCITPostFailedAction = new PopupWindowShowAction(components);
            AuthoriseCITPostFailedTxAction.AcceptButtonCaption = "Yes";
            AuthoriseCITPostFailedTxAction.CancelButtonCaption = "No";
            AuthoriseCITPostFailedTxAction.Caption = "Authorise";
            AuthoriseCITPostFailedTxAction.ConfirmationMessage = "The CIT(s) will be posted to core banking. Continue?";
            AuthoriseCITPostFailedTxAction.Id = "124344A3-8D30-42C8-A157-971D074C96DC";
            AuthoriseCITPostFailedTxAction.PaintStyle = ActionItemPaintStyle.CaptionAndImage;
            AuthoriseCITPostFailedTxAction.TargetObjectsCriteria = "[initialising_user].[id]!=CurrentUserId() && [is_complete] = False";
            AuthoriseCITPostFailedTxAction.TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueForAll;
            AuthoriseCITPostFailedTxAction.TargetObjectType = typeof(CITPosting);
            AuthoriseCITPostFailedTxAction.TargetViewId = "CITPosting_ListView_Pending";
            AuthoriseCITPostFailedTxAction.ToolTip = "The CIT(s) will be posted to core banking.";
            AuthoriseCITPostFailedTxAction.CustomizePopupWindowParams += new CustomizePopupWindowParamsEventHandler(AuthoriseCITPostFailedTxAction_CustomizePopupWindowParams);
            AuthoriseCITPostFailedTxAction.Execute += new PopupWindowShowActionExecuteEventHandler(AuthoriseCITPostFailedTxAction_Execute);
            RejectCITPostFailedTxAction.AcceptButtonCaption = "Yes";
            RejectCITPostFailedTxAction.CancelButtonCaption = "No";
            RejectCITPostFailedTxAction.Caption = "Reject";
            RejectCITPostFailedTxAction.ConfirmationMessage = "Reject the CIT(s) from posting to core banking. Continue?";
            RejectCITPostFailedTxAction.Id = "B1A5DE04-AF90-44F6-B7F1-EB369EF85FE5";
            RejectCITPostFailedTxAction.PaintStyle = ActionItemPaintStyle.CaptionAndImage;
            RejectCITPostFailedTxAction.TargetObjectsCriteria = "[initialising_user].[id]!=CurrentUserId() && [is_complete] = False";
            RejectCITPostFailedTxAction.TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueForAll;
            RejectCITPostFailedTxAction.TargetObjectType = typeof(CITPosting);
            RejectCITPostFailedTxAction.TargetViewId = "CITPosting_ListView_Pending";
            RejectCITPostFailedTxAction.ToolTip = "Reject the CIT(s) from posting to core banking.";
            RejectCITPostFailedTxAction.CustomizePopupWindowParams += new CustomizePopupWindowParamsEventHandler(RejectCITPostFailedTxAction_CustomizePopupWindowParams);
            RejectCITPostFailedTxAction.Execute += new PopupWindowShowActionExecuteEventHandler(RejectCITPostFailedTxAction_Execute);
            BlockCITPostFailedAction.AcceptButtonCaption = "Yes";
            BlockCITPostFailedAction.CancelButtonCaption = "No";
            BlockCITPostFailedAction.Caption = "Block!";
            BlockCITPostFailedAction.ConfirmationMessage = "Block the CIT(s) from posting to core banking. Blocked CIT will never be allowed to post through the Web Portal. Continue?";
            BlockCITPostFailedAction.Id = "9FF7A1AE-B6D5-4B62-8BBE-C382E5765B78";
            BlockCITPostFailedAction.PaintStyle = ActionItemPaintStyle.CaptionAndImage;
            BlockCITPostFailedAction.TargetObjectsCriteria = "[initialising_user].[id]!=CurrentUserId() && [is_complete] = False";
            BlockCITPostFailedAction.TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueForAll;
            BlockCITPostFailedAction.TargetObjectType = typeof(CITPosting);
            BlockCITPostFailedAction.TargetViewId = "CITPosting_ListView_Pending";
            BlockCITPostFailedAction.ToolTip = "Block the CIT(s) from posting to core banking. Blocked CIT will never be allowed to post through the Web Portal.";
            BlockCITPostFailedAction.CustomizePopupWindowParams += new CustomizePopupWindowParamsEventHandler(BlockCITPostFailedAction_CustomizePopupWindowParams);
            BlockCITPostFailedAction.Execute += new PopupWindowShowActionExecuteEventHandler(BlockCITPostFailedAction_Execute);
            Actions.Add(AuthoriseCITPostFailedTxAction);
            Actions.Add(RejectCITPostFailedTxAction);
            Actions.Add(BlockCITPostFailedAction);
            TargetObjectType = typeof(CITPosting);
            TargetViewId = "CITPosting_ListView_Pending";
            TargetViewType = ViewType.ListView;
            TypeOfView = typeof(ListView);
        }
    }
}
