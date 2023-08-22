using CashSwift.Finacle.Integration.CQRS.Helpers;
using CashSwift.Finacle.Integration.CQRS.Services.IServices;
using CashSwift.Finacle.Integration.DataAccess.Entities;
using CashSwift.Finacle.Integration.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CashSwift.Finacle.Integration.DataAccess;
using CashSwift.Finacle.Integration.Modules;
using CashSwift.Library.Standard.Logging;
using CashSwift.Finacle.Integration.DataAccess.Dapper;
using CashSwift.Finacle.Integration.Models.AccountValidation;
using CashSwift.API.Messaging.Integration.ServerPing;
using CashSwift.API.Messaging.Integration.Validations.AccountNumberValidations;
using CashSwift.Finacle.Integration.Filters;

namespace CashSwift.Finacle.Integration.Controllers.v2
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class MonitoringController : ControllerBase
    {
        private readonly ICashSwiftAPILogger Log;
        private readonly SOAServerConfiguration _soaServerConfiguration;
        private IIntegrationDataAccess DBContext;

        public MonitoringController(IOptionsMonitor<SOAServerConfiguration> optionsMonitor, ICashSwiftAPILogger logger, IConfiguration configuration)
        {
            Log = logger;
            _soaServerConfiguration = optionsMonitor.CurrentValue;
            DBContext = new IntegrationDataAccess(configuration);
        }

        [Route("ServerPing")]
        [MapToApiVersion("2.0")]
        [HMACAuthentication]
        [HttpPost]
        public async Task<IntegrationServerPingResponse> ServerPingAsync(IntegrationServerPingRequest request)
        {
            if (!_soaServerConfiguration.IsDebug)
            {
                IntegrationServerPingResponse result = new IntegrationServerPingResponse
                {
                    AppID = request.AppID,
                    AppName = request.AppName,
                    RequestID = request.MessageID,
                    SessionID = request.SessionID,
                    MessageID = Guid.NewGuid().ToString(),
                    MessageDateTime = DateTime.Now,
                    ServerOnline = false,
                    IsSuccess = false
                };
                try
                {
                    Log.Debug(request.SessionID, request.MessageID, request.AppName, GetType().Name, "GetCoreBankingStatus", "Request", "requestID={0}, deviceID={1}", request.MessageID, request.AppName);
                    DateTime pingTimeLimit = DateTime.Now.AddSeconds(Math.Max(_soaServerConfiguration?.MonitoringConfiguration?.PingInterval ?? 20, 20) * -1);
                    PingResponse dbResponse = await DBContext.GetLatestPingRequestAsync("CB_Integration");
                    if (dbResponse == null)
                    {
                        goto IL_021a;
                    }
                    if (dbResponse != null && dbResponse.UpdateTime < pingTimeLimit)
                    {
                        goto IL_021a;
                    }
                    result = new IntegrationServerPingResponse
                    {
                        AppID = request.AppID,
                        AppName = request.AppName,
                        RequestID = request.MessageID,
                        SessionID = request.SessionID,
                        MessageID = Guid.NewGuid().ToString(),
                        MessageDateTime = DateTime.Now,
                        ServerOnline = dbResponse.ServerOnline,
                        IsSuccess = dbResponse.IsSuccess,
                        ServerErrorCode = dbResponse.ErrorCode,
                        ServerErrorMessage = dbResponse.ErrorMessage
                    };
                    goto end_IL_00ad;

                IL_021a:
                    Guid.NewGuid();
                    _ = DateTime.Now;
                    CoopAccountDetailsRequest coopAccountDetailsRequest = new CoopAccountDetailsRequest(new AccountNumberValidationRequest
                    {
                        AppID = request.AppID,
                        AppName = request.AppName,
                        SessionID = request.SessionID,
                        MessageID = request.MessageID,
                        MessageDateTime = request.MessageDateTime,
                        AccountNumber = _soaServerConfiguration?.MonitoringConfiguration.PingAccountNumber,
                        CoreBankingString = _soaServerConfiguration?.MonitoringConfiguration.CoreBankingString,
                        Currency = _soaServerConfiguration?.MonitoringConfiguration.Currency,
                        TransactionType = 0
                    }, _soaServerConfiguration!.AccountValidationConfiguration);
                    new PingResponse();
                    PingResponse pingResponse;
                    try
                    {
                        (CoopAccountDetailsResponse, string) tuple = await new SOACommunicationManager(_soaServerConfiguration, Log).SendToCoopAsync<CoopAccountDetailsResponse>(request, coopAccountDetailsRequest.RawXML, _soaServerConfiguration.AccountValidationConfiguration.ServerURI.ToURI());
                        result = new IntegrationServerPingResponse
                        {
                            AppID = request.AppID,
                            AppName = request.AppName,
                            RequestID = request.MessageID,
                            SessionID = request.SessionID,
                            MessageID = Guid.NewGuid().ToString(),
                            MessageDateTime = DateTime.Now,
                            ServerOnline = !string.IsNullOrWhiteSpace(tuple.Item2),
                            IsSuccess = !string.IsNullOrWhiteSpace(tuple.Item2)
                        };
                        pingResponse = new PingResponse
                        {
                            UpdateTime = DateTime.Now,
                            ErrorCode = result.ServerErrorCode,
                            ErrorMessage = result.ServerErrorMessage,
                            IsSuccess = result.IsSuccess,
                            ServerName = "CB_Integration",
                            ServerOnline = result.ServerOnline
                        };
                    }
                    catch (Exception ex)
                    {
                        result.ServerErrorCode = "500";
                        result.ServerErrorMessage = ex.MessageString();
                        pingResponse = new PingResponse
                        {
                            UpdateTime = DateTime.Now,
                            ErrorCode = "500",
                            ErrorMessage = ex.MessageString(200),
                            IsSuccess = false,
                            ServerName = "CB_Integration",
                            ServerOnline = false
                        };
                    }
                    if (dbResponse != null)
                    {
                        await DBContext.UpdatePingResponseAsync(pingResponse);
                    }
                    else
                    {
                        await DBContext.InsertPingResponseAsync(pingResponse);
                    }
                end_IL_00ad:;
                }
                catch (Exception ex2)
                {
                    Console.WriteLine(ex2.MessageString());
                    Log.Error(request.SessionID, request.MessageID, request.AppName, GetType().Name, "Error", "Error", ex2.MessageString());
                    result = new IntegrationServerPingResponse
                    {
                        AppID = request.AppID,
                        AppName = request.AppName,
                        RequestID = request.MessageID,
                        SessionID = request.SessionID,
                        MessageID = Guid.NewGuid().ToString(),
                        MessageDateTime = DateTime.Now,
                        ServerOnline = false,
                        IsSuccess = false,
                        ServerErrorCode = "-1",
                        ServerErrorMessage = ex2.MessageString()
                    };
                }
                Log.Debug(request.SessionID, request.MessageID, request.AppName, GetType().Name, "GetCoreBankingStatus", "Result", "requestID={0}, result={1}", request.MessageID, result);
                return result;
            }
            else
            {

                var result = new IntegrationServerPingResponse
                {
                    AppID = request.AppID,
                    AppName = request.AppName,
                    RequestID = request.MessageID,
                    SessionID = request.SessionID,
                    MessageID = Guid.NewGuid().ToString(),
                    MessageDateTime = DateTime.Now,
                    ServerOnline = true,
                    IsSuccess = true
                };
                Log.Debug(request.SessionID, request.MessageID, request.AppName, GetType().Name, "GetCoreBankingStatus", "Result", "requestID={0}, result={1}", request.MessageID, result);

                return result;
            }
        }
    }
}

