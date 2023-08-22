
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BSAccountFundsTransferServiceReference;
using System.Net;
using System.ServiceModel;
using System.Text;
using CashSwift.Finacle.Integration.CQRS.Helpers;
using MediatR;
using Microsoft.Extensions.Options;
using AccountDetailsRequestHeaderType = BSAccountDetailsServiceReference.RequestHeaderType;
using FundsTransferRequestHeaderType = BSAccountFundsTransferServiceReference.RequestHeaderType;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Dynamic;
using CashSwift.Finacle.Integration.Extensions;
using System.Text.RegularExpressions;
using CashSwift.API.Messaging.Models;
using CashSwift.Finacle.Integration.CQRS.Services.IServices;
using CashSwift.Finacle.Integration.Models.FundsTransfer;
using AccountNumberValidationRequestDto = CashSwift.Finacle.Integration.Models.AccountNumberValidationRequestDto;
using CashSwift.Library.Standard.Logging;

namespace CashSwift.Finacle.Integration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankingController : ControllerBase
    {

        private readonly IAccountManagerService _IAccountManagerService;
        private readonly ILogger<BankingController> _logger;
        private readonly ICashSwiftAPILogger Log;
        private readonly SOAServerConfiguration _soaServerConfiguration;
        public BankingController(ILogger<BankingController> logger, IMediator mediator, IOptionsMonitor<SOAServerConfiguration> optionsMonitor, IWebHostEnvironment hostingEnv, IAccountManagerService iAccountManagerService)
        {
            _logger = logger;
            _soaServerConfiguration = optionsMonitor.CurrentValue;
            _IAccountManagerService=iAccountManagerService;
        }
        [Route("[action]")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AccountValidate(AccountNumberValidationRequestDto accountNumberValidationRequest)
        {
            try
            {
                Models.AccountValidation.CheckAccountPermission_Result permissionResult = await _IAccountManagerService.CheckAccountPermissionAsync(accountNumberValidationRequest.TransactionType, accountNumberValidationRequest.AccountNumber, accountNumberValidationRequest.Language);
                if (!permissionResult.IsSuccess)
                {
                    return Ok(new AccounValidationResponseDto
                    {
                        IsSuccess=false,
                        PublicErrorCode="E_000",
                        PublicErrorMessage=permissionResult.PublicErrorMessage,
                        Envelope = new EnvelopeDto
                        {
                            Body= new BodyDto { },
                            Header= new HeaderDto
                            {
                                ResponseHeader= new Responseheader()
                                {

                                    CorrelationID = "",
                                    StatusCode = "E_000",
                                    StatusDescription = permissionResult.PublicErrorMessage,
                                    StatusMessages = new Statusmessages()
                                    {
                                        MessageCode = "000",
                                        MessageDescription = permissionResult.PublicErrorMessage
                                    }
                                }

                            }
                        },
                    });
                }

                if (_soaServerConfiguration.IsDebug)
                {
                    var random = new Random();
                    var list = new List<string>{
                        $"<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:tns27=\"urn://co-opbank.co.ke/TS/Finacle/AccountDetails.1.0\" xmlns:tns25=\"urn://co-opbank.co.ke/BS/Account/BSAccountDetails.3.0\">\r\n\r\n   <soapenv:Header>\r\n\r\n      <head:ResponseHeader xmlns:head=\"urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader\" xmlns:tns26=\"urn://co-opbank.co.ke/BS/Account/AccountDetails/Get/3.0\" xmlns:tns3=\"urn://co-opbank.co.ke/CommonServices/Data/Common\">\r\n\r\n         <tns3:CorrelationID>0f4f114b-1abd-475f-84b9-3ec7ff964641</tns3:CorrelationID>\r\n\r\n         <head:MessageID>c812d7c3-df60-467e-8e0b-e27345b3b700</head:MessageID>\r\n\r\n         <head:StatusCode>S_001</head:StatusCode>\r\n\r\n         <head:StatusDescription>Success</head:StatusDescription>\r\n\r\n         <head:StatusMessages xmlns:tns36=\"urn://co-opbank.co.ke/TS/Finacle/AccountDetails.1.0\" xmlns:tns37=\"urn://co-opbank.co.ke/TS/Finacle/AccountDetails/Get/1.0\" xmlns:tns4=\"urn://co-opbank.co.ke/CommonServices/Data/Common\" xmlns:ns3=\"http://www.finacle.com/fixml\">\r\n\r\n            <head:MessageCode>0</head:MessageCode>\r\n\r\n            <head:MessageDescription>SUCCESS</head:MessageDescription>\r\n\r\n            <head:MessageType/>\r\n\r\n         </head:StatusMessages>\r\n\r\n      </head:ResponseHeader>\r\n\r\n   </soapenv:Header>\r\n\r\n   <soapenv:Body>\r\n\r\n      <tns25:AccountDetailsResponse>\r\n\r\n         <tns25:AccountNumber>01108137816900</tns25:AccountNumber>\r\n\r\n         <tns25:AccountName>EDDAH WAMBUI WAWERU</tns25:AccountName>\r\n\r\n         <tns25:CurrencyCode>KES</tns25:CurrencyCode>\r\n\r\n         <tns25:JointAccount>SELF</tns25:JointAccount>\r\n\r\n         <tns25:ProductID>SBA</tns25:ProductID>\r\n\r\n        <tns25:ProductContextCode>YETRA</tns25:ProductContextCode>\r\n\r\n         <tns25:ProductName>YEA ACCOUNT</tns25:ProductName>\r\n\r\n         <tns25:BookedBalance>0.0</tns25:BookedBalance>\r\n\r\n         <tns25:BlockedBalance>2000.0</tns25:BlockedBalance>\r\n\r\n         <tns25:AvailableBalance>16511.43</tns25:AvailableBalance>\r\n\r\n         <tns25:BranchName>DIGO ROAD BRANCH</tns25:BranchName>\r\n\r\n         <tns25:BranchCode>1050</tns25:BranchCode>\r\n\r\n         <tns25:PhoneNumber>0723244121</tns25:PhoneNumber>\r\n\r\n         <tns25:CustomerCode>101378169</tns25:CustomerCode>\r\n\r\n         <tns25:Email>wambui.eddah@gmail.com</tns25:Email>\r\n\r\n         <tns25:Dormant>Y</tns25:Dormant>\r\n\r\n         <tns25:Stopped>N</tns25:Stopped>\r\n\r\n         <tns25:Closed>N</tns25:Closed>\r\n\r\n         <tns25:AccountRightsIndicator></tns25:AccountRightsIndicator>\r\n\r\n         <tns25:PostalAddress>MOMBASA</tns25:PostalAddress>\r\n\r\n         <tns25:Town>002</tns25:Town>\r\n\r\n         <tns25:OpenDate>2010-02-19T00:00:00.000</tns25:OpenDate>\r\n\r\n         <tns25:Status>Dormant</tns25:Status>\r\n\r\n      </tns25:AccountDetailsResponse>\r\n\r\n   </soapenv:Body>\r\n\r\n</soapenv:Envelope>",
                        $"<?xml version='1.0' encoding='UTF-8'?><soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:tns25=\"urn://co-opbank.co.ke/BS/Account/BSAccountDetails.3.0\" xmlns:tns27=\"urn://co-opbank.co.ke/TS/Finacle/AccountDetails.1.0\"><soapenv:Header><head:ResponseHeader xmlns:head=\"urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader\" xmlns:tns26=\"urn://co-opbank.co.ke/BS/Account/AccountDetails/Get/3.0\" xmlns:tns3=\"urn://co-opbank.co.ke/CommonServices/Data/Common\"><tns3:CorrelationID>2ed8ee75-33e6-4f76-8cac-2c20731a2dd9</tns3:CorrelationID><head:MessageID>888c42dc-2700-4ff7-92d3-134b49432c67</head:MessageID><head:StatusCode>S_001</head:StatusCode><head:StatusDescription>Success</head:StatusDescription><head:StatusDescriptionKey /><head:StatusMessages xmlns:tns36=\"urn://co-opbank.co.ke/TS/Finacle/AccountDetails.1.0\" xmlns:tns37=\"urn://co-opbank.co.ke/TS/Finacle/AccountDetails/Get/1.0\" xmlns:tns4=\"urn://co-opbank.co.ke/CommonServices/Data/Common\" xmlns:ns3=\"http://www.finacle.com/fixml\" xmlns:tns38=\"http://schemas.tibco.com/bw/pe/plugin/5.0/exceptions\" xmlns:tns39=\"http://schemas.tibco.com/bw/plugins/http/5.0/httpExceptions\"><head:MessageCode>0</head:MessageCode><head:MessageDescription>SUCCESS</head:MessageDescription><head:MessageType /></head:StatusMessages></head:ResponseHeader></soapenv:Header><soapenv:Body><tns25:AccountDetailsResponse><tns25:AccountNumber>01100989948000</tns25:AccountNumber><tns25:AccountName>CLINTON /PAULINE/ALICE/ REAGAN</tns25:AccountName><tns25:CurrencyCode>KES</tns25:CurrencyCode><tns25:JointAccount>JOINT</tns25:JointAccount><tns25:ProductID>SBA</tns25:ProductID><tns25:ProductContextCode>INSTR</tns25:ProductContextCode><tns25:ProductName>INSTANT ACCESS MIN BAL</tns25:ProductName><tns25:ClearedBalance>30670.0</tns25:ClearedBalance><tns25:BookedBalance>30670.0</tns25:BookedBalance><tns25:BlockedBalance>0.0</tns25:BlockedBalance><tns25:AvailableBalance>30670.0</tns25:AvailableBalance><tns25:BranchName>DIGO ROAD BRANCH</tns25:BranchName><tns25:BranchCode>1050</tns25:BranchCode><tns25:PhoneNumber>0710754030</tns25:PhoneNumber><tns25:CustomerCode>109899480</tns25:CustomerCode><tns25:Email>unknown@co-opbank.co.ke</tns25:Email><tns25:Dormant>N</tns25:Dormant><tns25:Stopped>N</tns25:Stopped><tns25:Closed>N</tns25:Closed><tns25:AccountRightsIndicator>F</tns25:AccountRightsIndicator><tns25:PostalAddress>P.O. BOX 2800</tns25:PostalAddress><tns25:Town>002</tns25:Town><tns25:OpenDate>2022-06-08T00:00:00.000</tns25:OpenDate><tns25:Status>Active</tns25:Status></tns25:AccountDetailsResponse></soapenv:Body></soapenv:Envelope>",
                        $"<?xml version='1.0' encoding='UTF-8'?><soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:tns25=\"urn://co-opbank.co.ke/BS/Account/BSAccountDetails.3.0\" xmlns:tns27=\"urn://co-opbank.co.ke/TS/Finacle/AccountDetails.1.0\"><soapenv:Header><head:ResponseHeader xmlns:head=\"urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader\" xmlns:tns26=\"urn://co-opbank.co.ke/BS/Account/AccountDetails/Get/3.0\" xmlns:tns3=\"urn://co-opbank.co.ke/CommonServices/Data/Common\"><tns3:CorrelationID>6b93140e-1a77-4a86-8878-999eaf5e0016</tns3:CorrelationID><head:MessageID>c2252fd0-2f32-40e1-82a2-4f3c5c021bb6</head:MessageID><head:StatusCode>S_001</head:StatusCode><head:StatusDescription>Success</head:StatusDescription><head:StatusDescriptionKey /><head:StatusMessages xmlns:tns36=\"urn://co-opbank.co.ke/TS/Finacle/AccountDetails.1.0\" xmlns:tns37=\"urn://co-opbank.co.ke/TS/Finacle/AccountDetails/Get/1.0\" xmlns:tns4=\"urn://co-opbank.co.ke/CommonServices/Data/Common\" xmlns:ns3=\"http://www.finacle.com/fixml\" xmlns:tns38=\"http://schemas.tibco.com/bw/pe/plugin/5.0/exceptions\" xmlns:tns39=\"http://schemas.tibco.com/bw/plugins/http/5.0/httpExceptions\"><head:MessageCode>0</head:MessageCode><head:MessageDescription>SUCCESS</head:MessageDescription><head:MessageType /></head:StatusMessages></head:ResponseHeader></soapenv:Header><soapenv:Body><tns25:AccountDetailsResponse><tns25:AccountNumber>01100989943300</tns25:AccountNumber><tns25:AccountName>ERICK / JACKLINE / BENTER</tns25:AccountName><tns25:CurrencyCode>KES</tns25:CurrencyCode><tns25:JointAccount>SELF</tns25:JointAccount><tns25:ProductID>SBA</tns25:ProductID><tns25:ProductContextCode>INSTR</tns25:ProductContextCode><tns25:ProductName>INSTANT ACCESS MIN BAL</tns25:ProductName><tns25:ClearedBalance>10660.0</tns25:ClearedBalance><tns25:BookedBalance>10660.0</tns25:BookedBalance><tns25:BlockedBalance>0.0</tns25:BlockedBalance><tns25:AvailableBalance>10660.0</tns25:AvailableBalance><tns25:BranchName>DIGO ROAD BRANCH</tns25:BranchName><tns25:BranchCode>1050</tns25:BranchCode><tns25:PhoneNumber>0729453066</tns25:PhoneNumber><tns25:CustomerCode>109899433</tns25:CustomerCode><tns25:Email>eoma6261@gmail.com</tns25:Email><tns25:Dormant>N</tns25:Dormant><tns25:Stopped>N</tns25:Stopped><tns25:Closed>N</tns25:Closed><tns25:AccountRightsIndicator>C</tns25:AccountRightsIndicator><tns25:PostalAddress>P.O. BOX 83</tns25:PostalAddress><tns25:Town>411</tns25:Town><tns25:OpenDate>2022-05-16T00:00:00.000</tns25:OpenDate><tns25:Status>Active</tns25:Status></tns25:AccountDetailsResponse></soapenv:Body></soapenv:Envelope>",
                        $"<?xml version='1.0' encoding='UTF-8'?><soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:tns25=\"urn://co-opbank.co.ke/BS/Account/BSAccountDetails.3.0\" xmlns:tns27=\"urn://co-opbank.co.ke/TS/Finacle/AccountDetails.1.0\"><soapenv:Header><head:ResponseHeader xmlns:head=\"urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader\" xmlns:tns26=\"urn://co-opbank.co.ke/BS/Account/AccountDetails/Get/3.0\" xmlns:tns3=\"urn://co-opbank.co.ke/CommonServices/Data/Common\"><tns3:CorrelationID>db166433-16ae-4acb-a336-7fa6cbe79c99</tns3:CorrelationID><head:MessageID>bcfebf3d-6d99-4f51-ac5c-9137d5763489</head:MessageID><head:StatusCode>S_001</head:StatusCode><head:StatusDescription>Success</head:StatusDescription><head:StatusDescriptionKey /><head:StatusMessages xmlns:tns36=\"urn://co-opbank.co.ke/TS/Finacle/AccountDetails.1.0\" xmlns:tns37=\"urn://co-opbank.co.ke/TS/Finacle/AccountDetails/Get/1.0\" xmlns:tns4=\"urn://co-opbank.co.ke/CommonServices/Data/Common\" xmlns:ns3=\"http://www.finacle.com/fixml\" xmlns:tns38=\"http://schemas.tibco.com/bw/pe/plugin/5.0/exceptions\" xmlns:tns39=\"http://schemas.tibco.com/bw/plugins/http/5.0/httpExceptions\"><head:MessageCode>0</head:MessageCode><head:MessageDescription>SUCCESS</head:MessageDescription><head:MessageType /></head:StatusMessages></head:ResponseHeader></soapenv:Header><soapenv:Body><tns25:AccountDetailsResponse><tns25:AccountNumber>01100989939100</tns25:AccountNumber><tns25:AccountName>AMERSON MWENI / JACKSON KARISA</tns25:AccountName><tns25:CurrencyCode>KES</tns25:CurrencyCode><tns25:JointAccount>SELF</tns25:JointAccount><tns25:ProductID>SBA</tns25:ProductID><tns25:ProductContextCode>INSTR</tns25:ProductContextCode><tns25:ProductName>INSTANT ACCESS MIN BAL</tns25:ProductName><tns25:ClearedBalance>2560.0</tns25:ClearedBalance><tns25:BookedBalance>2560.0</tns25:BookedBalance><tns25:BlockedBalance>0.0</tns25:BlockedBalance><tns25:AvailableBalance>2560.0</tns25:AvailableBalance><tns25:BranchName>DIGO ROAD BRANCH</tns25:BranchName><tns25:BranchCode>1050</tns25:BranchCode><tns25:PhoneNumber>0718328171</tns25:PhoneNumber><tns25:CustomerCode>109899391</tns25:CustomerCode><tns25:Email>amwenny@gmail.com</tns25:Email><tns25:Dormant>N</tns25:Dormant><tns25:Stopped>N</tns25:Stopped><tns25:Closed>N</tns25:Closed><tns25:AccountRightsIndicator>T</tns25:AccountRightsIndicator><tns25:PostalAddress>P.O. BOX 226</tns25:PostalAddress><tns25:Town>863</tns25:Town><tns25:OpenDate>2022-04-30T00:00:00.000</tns25:OpenDate><tns25:Status>Active</tns25:Status></tns25:AccountDetailsResponse></soapenv:Body></soapenv:Envelope>",
                        $"<?xml version='1.0' encoding='UTF-8'?><soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:tns25=\"urn://co-opbank.co.ke/BS/Account/BSAccountDetails.3.0\" xmlns:tns27=\"urn://co-opbank.co.ke/TS/Finacle/AccountDetails.1.0\"><soapenv:Header><head:ResponseHeader xmlns:head=\"urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader\" xmlns:tns26=\"urn://co-opbank.co.ke/BS/Account/AccountDetails/Get/3.0\" xmlns:tns3=\"urn://co-opbank.co.ke/CommonServices/Data/Common\"><tns3:CorrelationID>bfa15619-01b8-42ef-b67b-6622831727f4</tns3:CorrelationID><head:MessageID>635510be-6b78-460f-88d5-c611032a14df</head:MessageID><head:StatusCode>S_001</head:StatusCode><head:StatusDescription>Success</head:StatusDescription><head:StatusDescriptionKey /><head:StatusMessages xmlns:tns36=\"urn://co-opbank.co.ke/TS/Finacle/AccountDetails.1.0\" xmlns:tns37=\"urn://co-opbank.co.ke/TS/Finacle/AccountDetails/Get/1.0\" xmlns:tns4=\"urn://co-opbank.co.ke/CommonServices/Data/Common\" xmlns:ns3=\"http://www.finacle.com/fixml\" xmlns:tns38=\"http://schemas.tibco.com/bw/pe/plugin/5.0/exceptions\" xmlns:tns39=\"http://schemas.tibco.com/bw/plugins/http/5.0/httpExceptions\"><head:MessageCode>0</head:MessageCode><head:MessageDescription>SUCCESS</head:MessageDescription><head:MessageType /></head:StatusMessages></head:ResponseHeader></soapenv:Header><soapenv:Body><tns25:AccountDetailsResponse><tns25:AccountNumber>01100989825700</tns25:AccountNumber><tns25:AccountName>ARNOLD KADIDE MBANDI</tns25:AccountName><tns25:CurrencyCode>KES</tns25:CurrencyCode><tns25:JointAccount>SELF</tns25:JointAccount><tns25:ProductID>SBA</tns25:ProductID><tns25:ProductContextCode>INSTR</tns25:ProductContextCode><tns25:ProductName>INSTANT ACCESS MIN BAL</tns25:ProductName><tns25:ClearedBalance>31500.0</tns25:ClearedBalance><tns25:BookedBalance>31500.0</tns25:BookedBalance><tns25:BlockedBalance>2880.0</tns25:BlockedBalance><tns25:AvailableBalance>28620.0</tns25:AvailableBalance><tns25:BranchName>DIGO ROAD BRANCH</tns25:BranchName><tns25:BranchCode>1050</tns25:BranchCode><tns25:PhoneNumber>0740657556</tns25:PhoneNumber><tns25:CustomerCode>109898257</tns25:CustomerCode><tns25:Email>kadidembandi@icloud.com</tns25:Email><tns25:Dormant>Y</tns25:Dormant><tns25:Stopped>N</tns25:Stopped><tns25:Closed>N</tns25:Closed><tns25:AccountRightsIndicator> </tns25:AccountRightsIndicator><tns25:PostalAddress>P.O.BOX 90400</tns25:PostalAddress><tns25:Town>002</tns25:Town><tns25:OpenDate>2020-12-03T00:00:00.000</tns25:OpenDate><tns25:Status>Dormant</tns25:Status></tns25:AccountDetailsResponse></soapenv:Body></soapenv:Envelope>",
                        };
                    int index = random.Next(list.Count);
                    string xmlDataSample = list[index];

                    _logger.LogInformation($"WebResponse: {xmlDataSample}");
                    XDocument docSample = XDocument.Parse(xmlDataSample);
                    string jsonTextSample = JsonConvert.SerializeXNode(docSample).Replace("ns7:", "").Replace("soapenv:", "").Replace("head:", "").Replace("tns4:", "").Replace("tns3:", "").Replace("tns25:", "").Replace("soapenv:", "");

                    Regex[] regexesSample = new Regex[]
                        {
                            new Regex("^.*xml.*$", RegexOptions.IgnoreCase),
                            new Regex("^.*xmlns.*$", RegexOptions.IgnoreCase),
                        };
                    string redactedJsonSample = CustomJsonConverterForType.RemoveSensitiveProperties(jsonTextSample, regexesSample);
                    var responseObjectSample = JsonConvert.DeserializeObject<AccounValidationResponseDto>(redactedJsonSample);
                    Type typeSample = responseObjectSample?.GetType();
                    AccounValidationResponseDto _responseObjectSample = new();
                    if (typeSample.Name == "AccounValidationResponseDto")
                    {
                        _responseObjectSample = responseObjectSample;
                        bool isDepositEnabled = _responseObjectSample.Envelope!=null&& _responseObjectSample.Envelope.Header.ResponseHeader.StatusCode == "S_001" && !(_responseObjectSample?.Envelope?.Body.AccountDetailsResponse.AccountRightsIndicator =="C" || _responseObjectSample?.Envelope?.Body.AccountDetailsResponse.AccountRightsIndicator =="T" || _responseObjectSample?.Envelope?.Body.AccountDetailsResponse.Closed =="Y"|| _responseObjectSample?.Envelope?.Body.AccountDetailsResponse.Dormant =="Y");
                        _responseObjectSample.IsSuccess = isDepositEnabled;
                        _responseObjectSample.PublicErrorMessage =isDepositEnabled ? _responseObjectSample?.Envelope?.Header.ResponseHeader.StatusDescription : "Validation Failed";
                        _responseObjectSample.PublicErrorCode = isDepositEnabled ? _responseObjectSample?.Envelope?.Header.ResponseHeader.StatusCode : "E_000";
                        responseObjectSample = _responseObjectSample;
                    }
                    return Ok(responseObjectSample);
                }
                var MessageReference = Guid.NewGuid().ToString().ToLower();
                var CreationTimestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");

                AccountDetailsRequestHeaderType requestHeaderType = new()
                {
                    CreationTimestamp = Convert.ToDateTime(CreationTimestamp),
                    CorrelationID = Guid.NewGuid().ToString().ToLower(),
                    MessageID = Guid.NewGuid().ToString().ToLower(),
                    Credentials = new BSAccountDetailsServiceReference.CredentialsType()
                    {
                        SystemCode = _soaServerConfiguration.AccountValidationConfiguration.SystemCode,
                        BankID = _soaServerConfiguration.AccountValidationConfiguration.BankID
                    }
                };

                var soapRequest = $"<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:mes=\"urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader\" xmlns:com=\"urn://co-opbank.co.ke/CommonServices/Data/Common\" xmlns:bsac=\"urn://co-opbank.co.ke/BS/Account/BSAccountDetails.3.0\">\r\n   <soapenv:Header>\r\n      <mes:RequestHeader>\r\n         <com:CreationTimestamp>{CreationTimestamp}</com:CreationTimestamp>\r\n         <com:CorrelationID>{requestHeaderType.CorrelationID}</com:CorrelationID>\r\n         <mes:FaultTO/>\r\n         <mes:MessageID>{requestHeaderType.MessageID}</mes:MessageID>\r\n         <mes:ReplyTO/>\r\n         <mes:Credentials>\r\n            <mes:SystemCode>{requestHeaderType.Credentials.SystemCode}</mes:SystemCode>\r\n            <mes:BankID>{requestHeaderType.Credentials.BankID}</mes:BankID>\r\n         </mes:Credentials>\r\n      </mes:RequestHeader>\r\n   </soapenv:Header>\r\n   <soapenv:Body>\r\n      <bsac:AccountDetailsRequest>\r\n         <bsac:AccountNumber>{accountNumberValidationRequest.AccountNumber}</bsac:AccountNumber>\r\n      </bsac:AccountDetailsRequest>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>";

                string xmlDataResponse = "";
                Guid.NewGuid().ToString();
                XmlDocument xmlDocument = new();
                xmlDocument.LoadXml(soapRequest);
                HttpWebRequest webRequest = GetAccountDetailsWebRequest(_soaServerConfiguration.AccountValidationConfiguration.ServerURI);

                using (Stream requestStream = webRequest.GetRequestStream())
                    xmlDocument.Save(requestStream);

                using (WebResponse response = webRequest.GetResponse())
                {
                    using (StreamReader streamReader = new(response.GetResponseStream()))
                    {
                        xmlDataResponse = streamReader.ReadToEnd();
                        _logger.LogInformation($"AccountValidate xmlDataResponse: {xmlDataResponse}");

                    }
                }
                _logger.LogInformation($"AccountValidate WebResponse: {xmlDataResponse}");
                XDocument doc = XDocument.Parse(xmlDataResponse);

                string jsonText = JsonConvert.SerializeXNode(doc).Replace("ns7:", "").Replace("soapenv:", "").Replace("head:", "").Replace("tns4:", "").Replace("tns3:", "").Replace("tns25:", "").Replace("soapenv:", "");

                Regex[] regexes = new Regex[]
                    {
                            new Regex("^.*xml.*$", RegexOptions.IgnoreCase),
                            new Regex("^.*xmlns.*$", RegexOptions.IgnoreCase),
                    };
                string redactedJson = CustomJsonConverterForType.RemoveSensitiveProperties(jsonText, regexes);
                var responseObject = JsonConvert.DeserializeObject<AccounValidationResponseDto>(redactedJson);
                Type type = responseObject?.GetType();
                AccounValidationResponseDto _responseObject = new();
                if (type.Name == "AccounValidationResponseDto")
                {
                    _responseObject = responseObject;
                    bool isDepositEnabled = _responseObject.Envelope!=null&& _responseObject.Envelope.Header.ResponseHeader.StatusCode == "S_001" && !(_responseObject?.Envelope?.Body.AccountDetailsResponse.AccountRightsIndicator =="C" || _responseObject?.Envelope?.Body.AccountDetailsResponse.AccountRightsIndicator =="T" || _responseObject?.Envelope?.Body.AccountDetailsResponse.Closed =="Y"|| _responseObject?.Envelope?.Body.AccountDetailsResponse.Dormant =="Y");
                    _responseObject.IsSuccess = isDepositEnabled;
                    _responseObject.PublicErrorMessage =isDepositEnabled ? _responseObject?.Envelope?.Header.ResponseHeader.StatusDescription : "Validation Failed";
                    _responseObject.PublicErrorCode = isDepositEnabled ? _responseObject?.Envelope?.Header.ResponseHeader.StatusCode : "E_000";
                    responseObject = _responseObject;
                }
                return Ok(responseObject);

            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountValidation Error StackTrace:{_soaServerConfiguration.AccountValidationConfiguration.ServerURI} : {ex.StackTrace}");
                return Ok(new
                {
                    message = "Account Validation Failed",
                    responseHeader = new
                    {
                        CorrelationID = "",
                        StatusCode = "E_000",
                        StatusDescription = ex.Message,
                        StatusMessages = new
                        {
                            MessageCode = "000",
                            MessageDescription = ex.Message
                        }

                    },
                    accountDetailsResponse = new
                    {

                    }
                });
            }
        }


        [Route("[action]")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> FundsTransfer([FromBody] FundsTransferRequest _ftReq)
        {
            try
            {

                if (_soaServerConfiguration.IsDebug)
                {
                    string xmlDataSample = $"<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:ns7=\"urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/4.0\">\r\n   <soapenv:Header>\r\n      <head:ResponseHeader xmlns:head=\"urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader\" xmlns:tns4=\"urn://co-opbank.co.ke/CommonServices/Data/Common\" xmlns:ns3=\"http://www.finacle.com/fixml\">\r\n         <tns4:CorrelationID>9f0ceafe-1924-464c-9dad-a30af625b2ff</tns4:CorrelationID>\r\n         <head:MessageID>d435cdba-0e34-427e-9044-72e0b815abfc</head:MessageID>\r\n         <head:StatusCode>S_001</head:StatusCode>\r\n         <head:StatusDescription>Success</head:StatusDescription>\r\n         <head:StatusMessages>\r\n            <head:MessageCode>0</head:MessageCode>\r\n            <head:MessageDescription>SUCCESS</head:MessageDescription>\r\n            <head:MessageType/>\r\n         </head:StatusMessages>\r\n      </head:ResponseHeader>\r\n   </soapenv:Header>\r\n   <soapenv:Body>\r\n      <ns7:FundsTransfer>\r\n         <ns7:MessageReference>d435cdba-0e34-427e-9044-72e0b815abfc</ns7:MessageReference>\r\n         <ns7:SystemCode>070</ns7:SystemCode>\r\n         <ns7:TransactionDatetime>{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")}</ns7:TransactionDatetime>\r\n         <ns7:ValueDate>{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")}</ns7:ValueDate>\r\n         <ns7:TransactionID>CB0001072_20220818</ns7:TransactionID>\r\n         <ns7:TransactionType>T</ns7:TransactionType>\r\n         <ns7:TransactionSubType>BI</ns7:TransactionSubType>\r\n         <ns7:TransactionResponseDetails>\r\n            <ns7:Remarks>CB0001072</ns7:Remarks>\r\n         </ns7:TransactionResponseDetails>\r\n         <ns7:TransactionItems>\r\n            <ns7:TransactionItem>\r\n               <ns7:TransactionReference>213138544034</ns7:TransactionReference>\r\n               <ns7:TransactionItemKey>213138544035_1</ns7:TransactionItemKey>\r\n               <ns7:AccountNumber>01108188032100</ns7:AccountNumber>\r\n               <ns7:DebitCreditFlag>D</ns7:DebitCreditFlag>\r\n               <ns7:TransactionAmount>200.0</ns7:TransactionAmount>\r\n               <ns7:TransactionCurrency>KES</ns7:TransactionCurrency>\r\n               <ns7:Narrative>213138544035|T|01010070021064|7</ns7:Narrative>\r\n               <ns7:TransactionCode>A2A</ns7:TransactionCode>\r\n               <ns7:AvailableBalance>45002344.81</ns7:AvailableBalance>\r\n               <ns7:BookedBalance>0.0</ns7:BookedBalance>\r\n               <ns7:TemporaryODDetails/>\r\n            </ns7:TransactionItem>\r\n            <ns7:TransactionItem>\r\n               <ns7:TransactionReference>213138544034</ns7:TransactionReference>\r\n               <ns7:TransactionItemKey>213138544035_2</ns7:TransactionItemKey>\r\n               <ns7:AccountNumber>01148743633300</ns7:AccountNumber>\r\n               <ns7:DebitCreditFlag>C</ns7:DebitCreditFlag>\r\n               <ns7:TransactionAmount>200.0</ns7:TransactionAmount>\r\n               <ns7:TransactionCurrency>KES</ns7:TransactionCurrency>\r\n               <ns7:Narrative>213138544035|T|01010070021064|7</ns7:Narrative>\r\n               <ns7:TransactionCode>A2A</ns7:TransactionCode>\r\n               <ns7:AvailableBalance>2126189.28</ns7:AvailableBalance>\r\n               <ns7:BookedBalance>0.0</ns7:BookedBalance>\r\n               <ns7:TemporaryODDetails/>\r\n            </ns7:TransactionItem>\r\n         </ns7:TransactionItems>\r\n         <ns7:TransactionCharges>\r\n            <ns7:Charge>\r\n               <ns7:EventType/>\r\n               <ns7:EventId/>\r\n            </ns7:Charge>\r\n         </ns7:TransactionCharges>\r\n         <ns7:TransactionItem>\r\n            <ns7:ValueDate>2022-08-01T17:32:24.05</ns7:ValueDate>\r\n         </ns7:TransactionItem>\r\n      </ns7:FundsTransfer>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>";

                    _logger.LogInformation($"WebResponse: {xmlDataSample}");
                    XDocument docSample = XDocument.Parse(xmlDataSample);
                    string jsonTextSample = JsonConvert.SerializeXNode(docSample).Replace("ns7:", "").Replace("soapenv:", "").Replace("head:", "").Replace("tns4:", "").Replace("tns3:", "").Replace("tns25:", "").Replace("soapenv:", "");

                    Regex[] regexesSample = new Regex[]
                        {
                            new Regex("^.*xml.*$", RegexOptions.IgnoreCase),
                            new Regex("^.*xmlns.*$", RegexOptions.IgnoreCase),
                        };
                    string redactedJsonSample = CustomJsonConverterForType.RemoveSensitiveProperties(jsonTextSample, regexesSample);
                    dynamic envelopeSample = JsonConvert.DeserializeObject<ExpandoObject>(redactedJsonSample);
                    return Ok(new
                    {
                        message = "Transaction Success",
                        result = envelopeSample
                    });
                }

                var remoteAddress = new EndpointAddress(_soaServerConfiguration.PostConfiguration.ServerURI);
                var MessageReference = Guid.NewGuid().ToString().ToLower();
                FundsTransferRequestHeaderType requestHeaderType = new()
                {
                    CreationTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")),
                    CorrelationID = Guid.NewGuid().ToString().ToLower(),
                    MessageID = Guid.NewGuid().ToString().ToLower(),
                    Credentials = new CredentialsType()
                    {
                        SystemCode = _ftReq.SystemCode_Cred,
                        BankID = _ftReq.BankID
                    }
                };
                FundsTransferTypeTransactionItem[] typeTransactionItemArray = new FundsTransferTypeTransactionItem[2]
                {
                          new FundsTransferTypeTransactionItem()
                          {
                            TransactionReference = $"{_ftReq.TransactionReference_Dr}",
                            TransactionItemKey ="1",
                            AccountNumber = _ftReq.AccountNumber_Dr,
                            DebitCreditFlag = "D",
                            TransactionAmount = _ftReq.TransactionAmount_Dr,
                            TransactionCurrency = _ftReq.TransactionCurrency_Dr,
                            Narrative = _ftReq.Narrative_Dr,
                            TransactionCode="A2A"
                          },
                          new FundsTransferTypeTransactionItem()
                          {
                            TransactionReference = $"{_ftReq.TransactionReference_Cr}",
                            TransactionItemKey ="2",
                            AccountNumber = _ftReq.AccountNumber_Cr,
                            DebitCreditFlag = "C",
                            TransactionAmount = _ftReq.TransactionAmount_Cr,
                            TransactionCurrency = _ftReq.TransactionCurrency_Cr,
                            Narrative = _ftReq.Narrative_Cr,
                            TransactionCode="A2A"
                          }
                };
                FundsTransferType fundsTransferType = new()
                {
                    SystemCode = _ftReq.SystemCode_FT,
                    MessageReference = MessageReference,
                    TransactionType = _ftReq.TransactionType,
                    TransactionSubType = _ftReq.TransactionSubType,
                    TransactionDatetime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")),
                    TransactionDatetimeSpecified = true,
                    ValueDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")),
                    ValueDateSpecified = true,
                    TransactionItems = typeTransactionItemArray
                };
                var TransactionDatetime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");

                var txnNarrative = $"CDM Cash Deposit~{_ftReq.AccountNumber_Dr}~{_ftReq.AccountNumber_Cr}~{_ftReq.Narrative_Dr}~{_ftReq.CDM_Number}";

                var soapRequest = $"<Envelope xmlns=\"http://schemas.xmlsoap.org/soap/envelope/\">\r\n   <Header>\r\n      <wstxns1:RequestHeader xmlns:wstxns1=\"urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader\">\r\n         <wstxns2:CreationTimestamp xmlns:wstxns2=\"urn://co-opbank.co.ke/CommonServices/Data/Common\">{TransactionDatetime}</wstxns2:CreationTimestamp>\r\n         <wstxns3:CorrelationID xmlns:wstxns3=\"urn://co-opbank.co.ke/CommonServices/Data/Common\">9f0ceafe-1924-464c-9dad-a30af625b2ff</wstxns3:CorrelationID>\r\n         <wstxns1:MessageID>{MessageReference}</wstxns1:MessageID>\r\n         <wstxns1:Credentials>\r\n            <wstxns1:SystemCode>{_ftReq.SystemCode_Cred}</wstxns1:SystemCode>\r\n            <wstxns1:BankID>{_ftReq.BankID}</wstxns1:BankID>\r\n         </wstxns1:Credentials>\r\n      </wstxns1:RequestHeader>\r\n   </Header>\r\n   <Body>\r\n      <wstxns4:FundsTransfer xmlns:wstxns4=\"urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/4.0\">\r\n         <wstxns4:MessageReference>{_ftReq.TransactionReference_Cr}</wstxns4:MessageReference>\r\n         <wstxns4:SystemCode>{fundsTransferType.SystemCode}</wstxns4:SystemCode>\r\n         <wstxns4:TransactionDatetime>{TransactionDatetime}</wstxns4:TransactionDatetime>\r\n         <wstxns4:ValueDate>{TransactionDatetime}</wstxns4:ValueDate>\r\n         <wstxns4:TransactionType>{_soaServerConfiguration.PostConfiguration.TransactionType}</wstxns4:TransactionType>\r\n         <wstxns4:TransactionSubType>{_soaServerConfiguration.PostConfiguration.TransactionSubType}</wstxns4:TransactionSubType>\r\n         <wstxns4:TransactionItems>\r\n            <wstxns4:TransactionItem>\r\n               <wstxns4:TransactionReference>{_ftReq.TransactionReference_Dr}</wstxns4:TransactionReference>\r\n               <wstxns4:TransactionItemKey>{_ftReq.TransactionReference_Dr}_1</wstxns4:TransactionItemKey>\r\n               <wstxns4:AccountNumber>{_ftReq.AccountNumber_Dr}</wstxns4:AccountNumber>\r\n               <wstxns4:DebitCreditFlag>D</wstxns4:DebitCreditFlag>\r\n               <wstxns4:TransactionAmount>{_ftReq.TransactionAmount_Dr}</wstxns4:TransactionAmount>\r\n               <wstxns4:TransactionCurrency>{_ftReq.TransactionCurrency_Dr}</wstxns4:TransactionCurrency>\r\n               <wstxns4:Narrative>{txnNarrative}</wstxns4:Narrative>\r\n               <wstxns4:TransactionCode>{_soaServerConfiguration.PostConfiguration.TransactionCode}</wstxns4:TransactionCode>\r\n            </wstxns4:TransactionItem>\r\n            <wstxns4:TransactionItem>\r\n               <wstxns4:TransactionReference>{_ftReq.TransactionReference_Cr}</wstxns4:TransactionReference>\r\n               <wstxns4:TransactionItemKey>{_ftReq.TransactionReference_Cr}_2</wstxns4:TransactionItemKey>\r\n               <wstxns4:AccountNumber>{_ftReq.AccountNumber_Cr}</wstxns4:AccountNumber>\r\n               <wstxns4:DebitCreditFlag>C</wstxns4:DebitCreditFlag>\r\n               <wstxns4:TransactionAmount>{_ftReq.TransactionAmount_Cr}</wstxns4:TransactionAmount>\r\n               <wstxns4:TransactionCurrency>{_ftReq.TransactionCurrency_Cr}</wstxns4:TransactionCurrency>\r\n               <wstxns4:Narrative>{txnNarrative}</wstxns4:Narrative>\r\n               <wstxns4:TransactionCode>{_soaServerConfiguration.PostConfiguration.TransactionCode}</wstxns4:TransactionCode>\r\n            </wstxns4:TransactionItem>\r\n         </wstxns4:TransactionItems>\r\n         <wstxns4:TransactionCharges>\r\n            <wstxns4:Charge>\r\n               <wstxns4:EventType/>\r\n               <wstxns4:EventId/>\r\n            </wstxns4:Charge>\r\n         </wstxns4:TransactionCharges>\r\n      </wstxns4:FundsTransfer>\r\n   </Body>\r\n</Envelope>";
                string xmlData = "";

                _logger.LogInformation($"FundsTransfer request: {soapRequest}");

                Guid.NewGuid().ToString();
                XmlDocument xmlDocument = new();
                xmlDocument.LoadXml(soapRequest);
                HttpWebRequest webRequest = CreateWebRequest(_soaServerConfiguration.PostConfiguration.ServerURI);

                using (Stream requestStream = webRequest.GetRequestStream())
                    xmlDocument.Save(requestStream);
                using (WebResponse response = webRequest.GetResponse())
                {
                    using (StreamReader streamReader = new(response.GetResponseStream()))
                    {
                        xmlData = streamReader.ReadToEnd();
                        _logger.LogInformation($"FundsTransfer response: {xmlData}");

                    }
                }
                _logger.LogInformation($"WebResponse Exc: {xmlData}");
                XDocument doc = XDocument.Parse(xmlData);

                string jsonText = JsonConvert.SerializeXNode(doc).Replace("ns7:", "").Replace("soapenv:", "").Replace("head:", "").Replace("tns4:", "").Replace("tns3:", "").Replace("tns25:", "").Replace("soapenv:", "");

                Regex[] regexes = new Regex[]
                    {
                            new Regex("^.*xml.*$", RegexOptions.IgnoreCase),
                            new Regex("^.*xmlns.*$", RegexOptions.IgnoreCase),
                    };
                string redactedJson = CustomJsonConverterForType.RemoveSensitiveProperties(jsonText, regexes);
                dynamic envelope = JsonConvert.DeserializeObject<ExpandoObject>(redactedJson);
                return Ok(new
                {
                    message = "Request successfully processed ",
                    result = envelope
                });

            }
            catch (Exception ex)
            {
                _logger.LogError($"FundsTransfer Error StackTrace: {_soaServerConfiguration.PostConfiguration.ServerURI}: {ex.StackTrace}");
                return Ok(new
                {
                    message = "Funds Transfer Failed",
                    responseHeader = new
                    {
                        CorrelationID = "",
                        StatusCode = "E_000",
                        StatusDescription = ex.Message,
                        StatusMessages = new
                        {
                            MessageCode = "000",
                            MessageDescription = ex.Message
                        }

                    },
                    accountDetailsResponse = new
                    {

                    }
                });
            }
        }

        private HttpWebRequest GetAccountDetailsWebRequest(string ServerURI)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(ServerURI);
            webRequest.Headers.Add("SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Headers["SOAPAction"] = "\"GetAccountDetails\"";
            webRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(_soaServerConfiguration.PostConfiguration.Username + ":" + _soaServerConfiguration.PostConfiguration.Password));
            webRequest.Method = "POST";
            return webRequest;
        }
        private HttpWebRequest CreateWebRequest(string ServerURI)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(ServerURI);
            webRequest.Headers.Add("SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Headers["SOAPAction"] = "\"Post\"";
            webRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(_soaServerConfiguration.PostConfiguration.Username + ":" + _soaServerConfiguration.PostConfiguration.Password));
            webRequest.Method = "POST";
            return webRequest;
        }

    }
}
