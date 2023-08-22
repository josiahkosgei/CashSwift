using CashSwift.Library.Standard.Logging;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Serialization;
using CashSwift.Finacle.Integration.Extensions;
using CashSwift.Finacle.Integration.CQRS.Helpers;
using CashSwift.API.Messaging;

namespace CashSwift.Finacle.Integration.Modules
{
    public class SOACommunicationManager
    {
        private SOAServerConfiguration _soaServerConfiguration;

        private readonly ICashSwiftAPILogger Log;

        //private CredentialCache CredentialCache = new CredentialCache();

        public SOACommunicationManager(SOAServerConfiguration integrationServerConfiguration, ICashSwiftAPILogger log)
        {
            _soaServerConfiguration = integrationServerConfiguration ?? throw new ArgumentNullException("integrationServerConfiguration");
            Log = log ?? throw new ArgumentNullException("log");
            ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, (RemoteCertificateValidationCallback)((object o, X509Certificate c, X509Chain ch, SslPolicyErrors er) => true));
        }

        internal async Task<(TResponseType ResponseObject, string ResponseXML)> SendToCoopAsync<TResponseType>(APIMessageBase request, string messageBody, Uri uri)
        {
            _ = request.MessageID;
            HttpWebResponse httpres = null;
            try
            {
                Log.Info(request.SessionID, request.MessageID, request.AppName, GetType().Name + ".SendToCoop<" + typeof(TResponseType).Name + ">()", "Attempt", "Coop", "Sending HTTPReq");
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.ContentType = "text/xml;charset=\"utf-8\"";
                httpWebRequest.Accept = "text/xml";
                httpWebRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(_soaServerConfiguration.PostConfiguration.Username + ":" + _soaServerConfiguration.PostConfiguration.Password));

                if (uri.OriginalString.Equals(_soaServerConfiguration.AccountValidationConfiguration.ServerURI))
                {
                    httpWebRequest.Headers["SOAPAction"] = "\"GetAccountDetails\"";
                }
                else if (uri.OriginalString.Equals(_soaServerConfiguration.PostConfiguration.ServerURI))
                {
                    if (_soaServerConfiguration.PostConfiguration.SOAVersion == 4.0)
                    {
                        httpWebRequest.Headers["SOAPAction"] = "\"Post\"";
                    }
                }
                httpWebRequest.Method = "POST";
                httpWebRequest.AllowWriteStreamBuffering = true;
                StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
                streamWriter.WriteLine(messageBody);
                streamWriter.Close();
                httpres = (HttpWebResponse)(await httpWebRequest.GetResponseAsync());
                if (httpres == null)
                {
                    throw new NullReferenceException("httpres is null in SendToCoop()");
                }
                Log.Info(request.SessionID, request.MessageID, request.AppName, GetType().Name + ".SendToCoopAsync", "CB Server Response", "CB Response", "Server returned: StatusCode = {0}>>StatusDescription = {1}", httpres.StatusCode, httpres.StatusDescription);
                var (item, item2) = GenerateResponseFromHttpWebResult<TResponseType>(request, httpres);
                return (item, item2);
            }
            catch (WebException ex)
            {
                WebResponse webResponse = ex?.Response;
                if (webResponse != null)
                {
                    using StreamReader streamReader = new StreamReader(webResponse.GetResponseStream());
                    Log.Error(request.SessionID, request.MessageID, request.AppName, GetType().Name + ".SendToCoop<" + typeof(TResponseType).Name + ">()", "Error", "Error", "{0} {1}", ex.MessageString(), streamReader.ReadToEnd());
                }
                throw;
            }
            catch (Exception ex2)
            {
                string text = ex2.MessageString();
                Console.WriteLine(text);
                Log.Error(request.SessionID, request.MessageID, request.AppName, GetType().Name + ".SendToCoop<" + typeof(TResponseType).Name + ">()", "Error", "Error", text);
                throw;
            }
            finally
            {
                httpres?.Close();
            }
        }

        private (TResponseObject ResponseObject, string ResponseXML) GenerateResponseFromHttpWebResult<TResponseObject>(APIMessageBase request, HttpWebResponse httpres)
        {
            Log.Debug(request.SessionID, request.MessageID, request.AppName, GetType().Name + ".GenerateResponseFromHttpWebResult<" + typeof(TResponseObject).Name + ">()", "Convert Coop Response", "Coop", "Inside Method");
            Stream responseStream = httpres.GetResponseStream();
            Encoding encoding = Encoding.GetEncoding("utf-8");
            StreamReader streamReader = new StreamReader(responseStream, encoding);
            try
            {
                string text = streamReader.ReadToEnd().Trim();
                Log.Info(request.SessionID, request.MessageID, request.AppName, GetType().Name + ".GenerateResponseFromHttpWebResult<" + typeof(TResponseObject).Name + ">()", "Convert Coop Response", "Coop", text);
                using StringReader textReader = new StringReader(text);
                TResponseObject item = (TResponseObject)new XmlSerializer(typeof(TResponseObject)).Deserialize(textReader);
                return (item, text);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                streamReader.Close();
                httpres.Close();
            }
        }
    }
}
