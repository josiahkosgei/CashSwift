using CashSwift.Finacle.Integration.CQRS.Helpers;
using CashSwift.Finacle.Integration.DataAccess.Dapper;
using CashSwift.Library.Standard.Logging;
using CashSwift.Library.Standard.Statuses;
using CashSwift.Library.Standard.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CashSwift.Finacle.Integration.Filters;
using CashSwift.Finacle.Integration.DataAccess.Entities;
using CashSwift.API.Messaging.Integration.Validations.ReferenceAccountNumberValidations;
//using CashSwift.API.Messaging.Integration.Validations.AccountNumberValidations;
using Microsoft.Extensions.Options;
using CashSwift.Finacle.Integration.Modules;
using CashSwift.Finacle.Integration.Models.AccountValidation;
using CashSwift.API.Messaging.Integration.Transactions;
using CashSwift.Finacle.Integration.Models.SOAIntegrationClasses;
using CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v1_0;
using CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1;
using CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_5;
using CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v4_0;
using CashSwift.API.Messaging.Integration.Validations.AccountNumberValidations;
using System;

namespace CashSwift.Finacle.Integration.Controllers.v2
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class BankingController : ControllerBase
    {

        private readonly ICashSwiftAPILogger Log;

        private IntegrationDataAccess DBContext;
        private readonly SOAServerConfiguration _soaServerConfiguration;
        private IConfiguration Configuration;
        public BankingController(ICashSwiftAPILogger logger, IOptionsMonitor<SOAServerConfiguration> optionsMonitor, IConfiguration configuration)
        {
            Configuration = configuration;
            _soaServerConfiguration = optionsMonitor.CurrentValue;
            Log = logger;
            DBContext = new IntegrationDataAccess(configuration);
        }
        #region AccountValidate

        [HttpPost]
        [MapToApiVersion("2.0")]
        [Route("AccountValidate")]
        [HMACAuthentication]
        public async Task<AccountNumberValidationResponse> ValidateAccountNumberAsync([FromBody] AccountNumberValidationRequest request)
        {

            AccountNumberValidationResponse accountNumberValidationResponse = new();
            try
            {
                if (!_soaServerConfiguration.IsDebug)
                {

                    accountNumberValidationResponse = await AccountValidatePrivateAsync(request);
                }
                else
                {
                    accountNumberValidationResponse= new AccountNumberValidationResponse
                    {

                        AppID = request.AppID,
                        AppName = request.AppName,
                        RequestID = request.MessageID,
                        SessionID = request.SessionID,
                        MessageDateTime = DateTime.Now,
                        MessageID = Guid.NewGuid().ToString(),
                        RequestedAccountNumber = request.AccountNumber,
                        AccountCurrency = request.Currency,
                        AccountName = "Test Ref A/C",
                        CanTransact = false,
                        IsSuccess = true,
                        PublicErrorCode = null,
                        PublicErrorMessage = null,
                        ServerErrorCode = null,
                        ServerErrorMessage = null
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error(request.SessionID, request.MessageID, request.AppName, GetType().Name, "AccountValidateAsync", "Error", ex.MessageString());
                Console.WriteLine(ex.MessageString());
                accountNumberValidationResponse = new AccountNumberValidationResponse
                {
                    AppID = request.AppID,
                    AppName = request.AppName,
                    CanTransact = false,
                    MessageDateTime = DateTime.Now,
                    RequestedCurrency = request.Currency,
                    RequestedAccountNumber = request.AccountNumber,
                    RequestID = request.MessageID,
                    MessageID = Guid.NewGuid().ToString(),
                    IsSuccess = false,
                    PublicErrorCode = "100",
                    PublicErrorMessage = "Account validation failed, contact administrator.",
                    ServerErrorCode = "-1",
                    ServerErrorMessage = ex.MessageString()
                };
            }
            Log.Info(request.SessionID, request.MessageID, request.AppName, GetType().Name, "Device Response", "AccountValidate", JsonConvert.SerializeObject(accountNumberValidationResponse));
            return accountNumberValidationResponse;
        }

        [HttpPost]
        [MapToApiVersion("2.0")]
        [Route("ReferenceAccountNumberValidate")]
        [HMACAuthentication]
        public async Task<ReferenceAccountNumberValidationResponse> ValidateReferenceAccountNumberAsync([FromBody] ReferenceAccountNumberValidationRequest request)
        {
            new ReferenceAccountNumberValidationResponse();
            DateTime now = DateTime.Now;
            _ = TimeSpan.Zero;
            TimeSpan zero = TimeSpan.Zero;
            Log.Info(request.SessionID, request.MessageID, request.AppName, GetType().Name, "ReferenceAccountNumber", "Validation", "RequestID: " + request.MessageID + ", AppName: " + request.AppName + " Account Number: " + request.AccountNumber + ", Reference Account: " + request.ReferenceAccountNumber + ", Account Type: " + request.CoreBankingString);
            ReferenceAccountNumberValidationResponse referenceAccountNumberValidationResponse;
            if (!_soaServerConfiguration.IsDebug)
            {
                try
                {
                    throw new NotImplementedException();
                }
                catch (Exception ex)
                {
                    Log.Error(request.SessionID, request.MessageID, request.AppName, GetType().Name, "Error", "Error", ex.MessageString());
                    Console.WriteLine(ex.MessageString());
                    referenceAccountNumberValidationResponse = new ReferenceAccountNumberValidationResponse
                    {
                        AccountName = null,
                        CanTransact = false,
                        MessageDateTime = DateTime.Now,
                        RequestID = request.MessageID,
                        SessionID = request.SessionID,
                        IsSuccess = false,
                        AppID = request.AppID,
                        AppName = request.AppName,
                        MessageID = Guid.NewGuid().ToString(),
                        RequestedReferenceAccountNumber = request.ReferenceAccountNumber,
                        RequestedAccountNumber = request.AccountNumber,
                        RequestedCurrency = request.Currency,
                        PublicErrorCode = "500",
                        PublicErrorMessage = "Validation failed. Please try again later",
                        ServerErrorCode = "-1",
                        ServerErrorMessage = ex.MessageString()
                    };
                }
            }
            else
            {
                referenceAccountNumberValidationResponse = new ReferenceAccountNumberValidationResponse
                {
                    AppID = request.AppID,
                    AppName = request.AppName,
                    RequestID = request.MessageID,
                    SessionID = request.SessionID,
                    MessageDateTime = DateTime.Now,
                    MessageID = Guid.NewGuid().ToString(),
                    RequestedAccountNumber = request.AccountNumber,
                    RequestedCurrency = request.Currency,
                    RequestedReferenceAccountNumber = request.ReferenceAccountNumber,
                    AccountName = "Test Ref A/C",
                    CanTransact = true,
                    IsSuccess = true,
                    PublicErrorCode = null,
                    PublicErrorMessage = null,
                    ServerErrorCode = null,
                    ServerErrorMessage = null
                };
            }
            TimeSpan timeSpan = DateTime.Now - now;
            Log.Info(request.SessionID, request.MessageID, request.AppName, GetType().Name, "ReferenceAccountNumber", "Result", "requestID={0}, result={1}, ref_time = {2}, CBDuration = {3}", request.MessageID, referenceAccountNumberValidationResponse, timeSpan, zero);
            return referenceAccountNumberValidationResponse;
        }

        internal async Task<CoopAccountDetailsRequest> GenerateCoopAccountNumberRequestAsync(AccountNumberValidationRequest request)
        {
            try
            {
                string messageID = request.MessageID;
                _ = request.MessageDateTime;
                CoopAccountDetailsRequest result = new(request, _soaServerConfiguration.AccountValidationConfiguration);
                await DBContext.InsertValidateAccountRequestAsync(new ValidateAccountRequest
                {
                    id = Guid.NewGuid(),
                    Session = request.SessionID,
                    Created = DateTime.Now,
                    RequestID = messageID,
                    Raw = result.RawXML
                });
                return result;
            }
            catch (Exception ex)
            {
                string value = $"{ex.Message}>>{ex.InnerException?.Message}>>{ex.InnerException?.InnerException?.Message}>stack>{ex.StackTrace}";
                Console.WriteLine(value);
                Log.Error(request.SessionID, request.MessageID, request.AppName, GetType().Name + ".GenerateCoopAccountNumberRequest", "Error", "Error", ex.MessageString(), ex.MessageString());
                throw;
            }
        }

        private async Task<AccountNumberValidationResponse> AccountValidatePrivateAsync(AccountNumberValidationRequest request)
        {
            new AccountNumberValidationResponse();
            AccountNumberValidationResponse accountNumberValidationResponse;
            try
            {
                Log.Info(request.SessionID, request.MessageID, request.AppName, GetType().Name, "Device Request", "AccountValidate", "requestID={0}\tAppName={1}\taccountNumber={2}\tcurrency={3}", request.MessageID, request.AppName, request.AccountNumber, request.Currency);
                CheckAccountPermission_Result checkAccountPermission_Result = await new AccountManager(DBContext).CheckAccountPermissionAsync(request.TransactionType, request.AccountNumber, request.Language);
                if (!checkAccountPermission_Result.IsSuccess)
                {
                    accountNumberValidationResponse = new AccountNumberValidationResponse
                    {
                        AppID = request.AppID,
                        AppName = request.AppName,
                        SessionID = request.SessionID,
                        MessageID = Guid.NewGuid().ToString(),
                        RequestID = request.MessageID,
                        RequestedCurrency = request.Currency,
                        RequestedAccountNumber = request.AccountNumber,
                        MessageDateTime = DateTime.Now,
                        AccountCurrency = null,
                        AccountName = null,
                        CanTransact = checkAccountPermission_Result.IsSuccess,
                        IsSuccess = checkAccountPermission_Result.IsSuccess,
                        PublicErrorCode = "100",
                        PublicErrorMessage = checkAccountPermission_Result.PublicErrorMessage,
                        ServerErrorCode = "0",
                        ServerErrorMessage = checkAccountPermission_Result.ServerErrorMessage
                    };
                }
                else
                {
                    CoopAccountDetailsRequest coopAccountDetailsRequest = await GenerateCoopAccountNumberRequestAsync(request);
                    (CoopAccountDetailsResponse, string) tuple = await new SOACommunicationManager(_soaServerConfiguration, Log).SendToCoopAsync<CoopAccountDetailsResponse>(request, coopAccountDetailsRequest.RawXML, _soaServerConfiguration.AccountValidationConfiguration.ServerURI.ToURI());
                    var (account, _) = tuple;
                    string text = account.RawXML = tuple.Item2;
                    account?.ValidateResponse();
                    Log.Trace(request.SessionID, request.MessageID, request.AppName, "BankingController", "Log Coop Result", "AccountValidatePrivateAsync", "Raw response = {0}>>Account = {1}", text, account);
                    Log.Trace(request.SessionID, request.MessageID, request.AppName, "BankingController", "InsertValidateAccountResponseAsync", "AccountValidatePrivateAsync", "Saving to database");
                    await DBContext.InsertValidateAccountResponseAsync(new ValidateAccountResponse
                    {
                        id = Guid.NewGuid(),
                        Session = request.SessionID,
                        Created = DateTime.Now,
                        RequestID = request.MessageID,
                        Raw = account.RawXML
                    });
                    Log.Trace(request.SessionID, request.MessageID, request.AppName, "BankingController", "Generating AccountNumberValidationResponse", "AccountValidatePrivateAsync", "Generating AccountNumberValidationResponse");
                    accountNumberValidationResponse = new AccountNumberValidationResponse
                    {
                        AppID = request.AppID,
                        AppName = request.AppName,
                        SessionID = request.SessionID,
                        MessageID = Guid.NewGuid().ToString(),
                        RequestID = request.MessageID,
                        RequestedCurrency = request.Currency,
                        RequestedAccountNumber = request.AccountNumber,
                        MessageDateTime = DateTime.Now,
                        AccountCurrency = account?.Currency,
                        AccountName = account?.AccountName,
                        CanTransact = account?.StatusCode == "S_001" && !(account?.Body.AccountDetailsResponse.AccountRightsIndicator =="C" || account?.Body.AccountDetailsResponse.AccountRightsIndicator =="T" || account?.Body.AccountDetailsResponse.Closed =="Y"|| account?.Body.AccountDetailsResponse.Dormant =="Y"),
                        IsSuccess = account?.StatusCode == "S_001" && !(account?.Body.AccountDetailsResponse.AccountRightsIndicator =="C" || account?.Body.AccountDetailsResponse.AccountRightsIndicator =="T" || account?.Body.AccountDetailsResponse.Closed =="Y"|| account?.Body.AccountDetailsResponse.Dormant =="Y"),
                        PublicErrorCode = account?.ValidationStatus,
                        PublicErrorMessage = account?.ValidationMessage,
                        ServerErrorCode = account?.StatusCode,
                        ServerErrorMessage = account?.StatusMessage
                    };
                    Log.Trace(request.SessionID, request.MessageID, request.AppName, "BankingController", "Generated AccountNumberValidationResponse", "AccountValidatePrivateAsync", "result = {0}", accountNumberValidationResponse);
                }
            }
            catch (CashSwiftException ex)
            {
                string serverErrorMessage = ex.ServerErrorMessage;
                Console.WriteLine(serverErrorMessage);
                Log.Error(request.SessionID, request.MessageID, request.AppName, GetType().Name, "Error", "Error", serverErrorMessage);
                accountNumberValidationResponse = new AccountNumberValidationResponse
                {
                    AppID = request.AppID,
                    AppName = request.AppName,
                    SessionID = request.SessionID,
                    MessageID = Guid.NewGuid().ToString(),
                    RequestID = request.MessageID,
                    MessageDateTime = DateTime.Now,
                    AccountCurrency = null,
                    AccountName = null,
                    RequestedCurrency = request.Currency,
                    RequestedAccountNumber = request.AccountNumber,
                    CanTransact = false,
                    IsSuccess = false,
                    PublicErrorCode = ex.PublicErrorCode,
                    PublicErrorMessage = ex.PublicErrorMessage,
                    ServerErrorCode = ex.ServerErrorCode,
                    ServerErrorMessage = ex.ServerErrorMessage
                };
            }
            catch (Exception ex2)
            {
                string text2 = ex2.MessageString();
                Console.WriteLine(text2);
                Log.Error(request.SessionID, request.MessageID, request.AppName, GetType().Name, "Error", "Error", text2);
                accountNumberValidationResponse = new AccountNumberValidationResponse
                {
                    AppID = request.AppID,
                    AppName = request.AppName,
                    SessionID = request.SessionID,
                    MessageID = Guid.NewGuid().ToString(),
                    RequestID = request.MessageID,
                    MessageDateTime = DateTime.Now,
                    AccountCurrency = null,
                    AccountName = null,
                    RequestedCurrency = request.Currency,
                    RequestedAccountNumber = request.AccountNumber,
                    CanTransact = false,
                    IsSuccess = false,
                    PublicErrorCode = "500",
                    PublicErrorMessage = "Validation failed. Contact administrator.",
                    ServerErrorCode = "500",
                    ServerErrorMessage = text2
                };
            }
            Log.Info(request.SessionID, request.MessageID, request.AppName, GetType().Name, "Device Response", "AccountValidate", "requestID={0}, result={1}", request.MessageID, accountNumberValidationResponse);
            return accountNumberValidationResponse;
        }

        #endregion
        #region FundsTransfer


        [HttpPost]
        [MapToApiVersion("2.0")]
        [Route("CITFundsTransfer")]
        [HMACAuthentication]
        public async Task<PostCITTransactionResponse> PostCITTransactionAsync(PostCITTransactionRequest request)
        {
            new PostCITTransactionResponse();
            try
            {
                PostTransactionResponse postTransactionResponse = await PostTransactionAsync(new PostTransactionRequest
                {
                    AppID = request.AppID,
                    AppName = request.AppName,
                    MessageID = request.MessageID,
                    Transaction = request.Transaction,
                    Language = request.Language,
                    MessageDateTime = request.MessageDateTime,
                    SessionID = request.SessionID
                });
                return new PostCITTransactionResponse
                {
                    AppID = postTransactionResponse.AppID,
                    AppName = postTransactionResponse.AppName,
                    IsSuccess = postTransactionResponse.IsSuccess,
                    ServerErrorCode = postTransactionResponse.ServerErrorCode,
                    ServerErrorMessage = postTransactionResponse.ServerErrorMessage,
                    MessageDateTime = postTransactionResponse.MessageDateTime,
                    MessageID = postTransactionResponse.MessageID,
                    PostResponseCode = postTransactionResponse.PostResponseCode,
                    PostResponseMessage = postTransactionResponse.PostResponseMessage,
                    PublicErrorCode = postTransactionResponse.PublicErrorCode,
                    PublicErrorMessage = postTransactionResponse.PublicErrorMessage,
                    RequestID = postTransactionResponse.RequestID,
                    SessionID = postTransactionResponse.SessionID,
                    TransactionDateTime = postTransactionResponse.TransactionDateTime,
                    TransactionID = postTransactionResponse.TransactionID
                };
            }
            catch (Exception ex)
            {
                return new PostCITTransactionResponse
                {
                    AppID = request.AppID,
                    AppName = request.AppName,
                    RequestID = request.MessageID,
                    SessionID = request.SessionID,
                    MessageDateTime = DateTime.Now,
                    MessageID = Guid.NewGuid().ToString(),
                    PostResponseCode = "500",
                    PostResponseMessage = "FAIL",
                    TransactionDateTime = DateTime.Now,
                    TransactionID = Guid.NewGuid().ToString().ToUpper(),
                    IsSuccess = true,
                    ServerErrorCode = $"{500:#}",
                    ServerErrorMessage = ex.MessageString()
                };
            }
        }

        [HttpPost]
        [MapToApiVersion("2.0")]
        [Route("FundsTransfer")]
        [HMACAuthentication]
        public async Task<PostTransactionResponse> PostTransactionAsync(PostTransactionRequest request)
        {
            DateTime post_start = DateTime.Now;
            TimeSpan zero = TimeSpan.Zero;
            TimeSpan cb_post_duration = TimeSpan.Zero;
            PostTransactionResponse transactionResponse1 = new();
            PostTransactionResponse transactionResponse2;
            if (!_soaServerConfiguration.IsDebug)
            {
                try
                {
                    ValidateAccountResponse accountResponseAsync = await DBContext.GetLatestValidateAccountResponseAsync(request.SessionID);
                    string tx_narration = string.Format("{0:0} {1} {2} {3} {4} {5}", request.DeviceReferenceNumber, request.DepositorPhone, request.Transaction.DeviceNumber, request.Transaction.CreditAccount.AccountNumber, request.Transaction.Narration, request.DepositorName).Left(100);

                    CoopPostResponse postResult = await CoopPostV4_0Async(request, accountResponseAsync, tx_narration, _soaServerConfiguration.PostConfiguration);

                    int num = await DBContext.InsertPostResponseAsync(new PostResponse()
                    {
                        id = Guid.NewGuid(),
                        Session = request.SessionID,
                        Created = DateTime.Now,
                        RequestID = request.MessageID,
                        Raw = JsonConvert.SerializeObject(postResult)
                    }) ? 1 : 0;
                    PostTransactionResponse transactionResponse3 = new()
                    {
                        AppID = request.AppID,
                        AppName = request.AppName,
                        SessionID = request.SessionID,
                        MessageDateTime = DateTime.Now,
                        RequestID = request.MessageID,
                        MessageID = postResult?.RequestUUID,
                        PostResponseCode = postResult?.PostResponseCode,
                        PostResponseMessage = postResult?.PostResponseMessage,
                        TransactionDateTime = postResult.TransactionDateTime,
                        TransactionID = postResult?.TransactionID,
                        IsSuccess = postResult.Success
                    };
                    transactionResponse2 = transactionResponse3;
                    postResult = null;
                }
                catch (CashSwiftException ex)
                {
                    Console.WriteLine(ex.MessageString());
                    Log.Error(request.SessionID, request.MessageID, request.AppName, GetType().Name, "Error", "Error", ex.MessageString());
                    PostTransactionResponse transactionResponse4 = new()
                    {
                        AppID = request.AppID,
                        AppName = request.AppName,
                        RequestID = request.MessageID,
                        SessionID = request.SessionID,
                        MessageDateTime = DateTime.Now,
                        MessageID = Guid.NewGuid().ToString(),
                        PostResponseCode = "500",
                        PostResponseMessage = "FAIL",
                        TransactionDateTime = DateTime.Now,
                        IsSuccess = true,
                        PublicErrorCode = ex.PublicErrorCode,
                        PublicErrorMessage = ex.PublicErrorMessage,
                        ServerErrorCode = ex.ServerErrorCode,
                        ServerErrorMessage = ex.ServerErrorMessage
                    };
                    transactionResponse2 = transactionResponse4;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.MessageString());
                    Log.Error(request.SessionID, request.MessageID, request.AppName, GetType().Name, "Error", "Error", ex.MessageString());
                    PostTransactionResponse transactionResponse5 = new()
                    {
                        AppID = request.AppID,
                        AppName = request.AppName,
                        RequestID = request.MessageID,
                        SessionID = request.SessionID,
                        MessageDateTime = DateTime.Now,
                        MessageID = Guid.NewGuid().ToString(),
                        PostResponseCode = "500",
                        PostResponseMessage = "FAIL",
                        TransactionDateTime = DateTime.Now,
                        IsSuccess = true,
                        ServerErrorCode = string.Format("{0:#}", 500),
                        ServerErrorMessage = ex.MessageString()
                    };
                    transactionResponse2 = transactionResponse5;
                }
            }
            else
            {
                PostTransactionResponse transactionResponse6 = new()
                {
                    AppID = request.AppID,
                    AppName = request.AppName,
                    RequestID = request.MessageID,
                    SessionID = request.SessionID,
                    MessageDateTime = DateTime.Now,
                    MessageID = Guid.NewGuid().ToString(),
                    TransactionID = Guid.NewGuid().ToString().ToUpper(),
                    TransactionDateTime = DateTime.Now,
                    IsSuccess = true,
                    PostResponseCode = "200",
                    PostResponseMessage = "SUCCESS",
                    PublicErrorCode = null,
                    PublicErrorMessage = null,
                    ServerErrorCode = null,
                    ServerErrorMessage = null
                };
                transactionResponse2 = transactionResponse6;
            }
            transactionResponse2.TransactionDateTime = DateTime.MinValue + cb_post_duration;
            Log.Info(request.SessionID, request.MessageID, request.AppName, GetType().Name, "PostTransaction", "Response", "requestID={0}, result={1}", request.MessageID, transactionResponse2);
            TimeSpan timeSpan = DateTime.Now - post_start;
            Log.Info(request.SessionID, request.MessageID, request.AppName, GetType().Name, "PostTransaction", "Report", "cb_post_duration={0}, post_duration={1}", cb_post_duration, timeSpan);
            return transactionResponse2;
        }

        private async Task<CoopPostRequestV4_0> GeneratePostRequestV4_0Async(PostTransactionRequest request, ValidateAccountResponse CoopAccount, string tx_narration)
        {
            CoopPostRequestV4_0 postRequestV25Async;
            try
            {
                string RequestUUID = Guid.NewGuid().ToString().ToUpper();
                DateTime now = DateTime.Now;
                Device device = await DBContext.GetDevice(request.DeviceID) ?? throw new CashSwiftException(string.Format("Device '{0}' is invalid", request.DeviceID));
                CoopPostRequestV4_0 result = new(request, (await DBContext.GetBranch(device.branch_id) ?? throw new CashSwiftException(string.Format("Branch '{0}' is invalid", device.branch_id))).branch_code, tx_narration, _soaServerConfiguration.PostConfiguration);
                int num = await DBContext.InsertPostRequestAsync(new PostRequest()
                {
                    id = Guid.NewGuid(),
                    Session = request.SessionID,
                    Created = DateTime.Now,
                    RequestID = RequestUUID,
                    Raw = result.RawXML
                }) ? 1 : 0;
                postRequestV25Async = result;
            }
            catch (CashSwiftException ex)
            {
                Console.WriteLine(ex.MessageString());
                Log.Error(request.SessionID, request.MessageID, request.AppName, GetType().Name + ".GeneratePostRequest", "Error", "Error", ex.MessageString());
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.MessageString());
                Log.Error(request.SessionID, request.MessageID, request.AppName, GetType().Name + ".GeneratePostRequest", "Error", "Error", ex.MessageString());
                throw;
            }
            return postRequestV25Async;
        }

        private async Task<CoopPostResponse> CoopPostV4_0Async(PostTransactionRequest request, ValidateAccountResponse CoopAccount, string tx_narration, PostConfiguration postConfiguration)
        {
            CoopPostRequestV4_0 postRequestV40Async = await GeneratePostRequestV4_0Async(request, CoopAccount, tx_narration);
            (CoopPostResponseV4_0, string) tuple = await new SOACommunicationManager(_soaServerConfiguration, Log).SendToCoopAsync<CoopPostResponseV4_0>(request, postRequestV40Async.RawXML, postConfiguration.ServerURI.ToURI());

            var (coopPostResponseV40, _) = tuple;
            string text = coopPostResponseV40.RawXML = tuple.Item2;


            return new CoopPostResponse()
            {
                RequestUUID = coopPostResponseV40?.RequestUUID ?? request.MessageID,
                PostResponseCode = coopPostResponseV40?.StatusCode,
                PostResponseMessage = coopPostResponseV40?.StatusMessage,
                TransactionDateTime = coopPostResponseV40.CBTransactionDate,
                TransactionID = coopPostResponseV40?.CBReference,
                Success = coopPostResponseV40.Success,
                Raw = text,
                StatusCode = coopPostResponseV40?.StatusCode
            };
        }

        #endregion
    }
}
