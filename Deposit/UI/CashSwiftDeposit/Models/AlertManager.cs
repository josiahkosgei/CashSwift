using CashSwift.Library.Standard.Logging;
using CashSwiftDataAccess.Data;
using CashSwiftDataAccess.Entities;
using CashSwiftDeposit.Utils.AlertClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CashSwiftDeposit.Models
{
    public class AlertManager
    {
        public static ICashSwiftLogger Log;
        private static string commserv_uri;
        private static Guid appID;
        private static byte[] AppKey;
        private static string appName;

        private IList<AlertMessageType> AllowedMessages { get; set; }

        private DepositorCommunicationService DepositorCommunicationService { get; set; }

        public AlertManager(
          ICashSwiftLogger logger,
          string CommServURI,
          Guid AppID,
          byte[] appKey,
          string AppName)
        {
            AlertManager.Log = logger;
            AlertManager.appID = AppID;
            AlertManager.AppKey = appKey;
            AlertManager.appName = AppName;
            AlertManager.commserv_uri = CommServURI;
            InitialiseAlertManager();
        }

        public void SendAlert(AlertBase alert)
        {
            try
            {
                alert?.SendAlert();
            }
            catch (Exception ex)
            {
            }
        }

        public void InitialiseAlertManager()
        {
            using (DepositorDBContext depositorDbContext = new DepositorDBContext())
            {
                AllowedMessages = depositorDbContext.AlertMessageTypes.Where(x => x.enabled == true).ToList();
                DepositorCommunicationService = DepositorCommunicationService.NewDepositorCommunicationService(AlertManager.commserv_uri, AlertManager.appID, AlertManager.AppKey, AlertManager.appName);
            }
        }
    }
}
