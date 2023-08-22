using CashSwiftDataAccess.Data;
using CashSwiftDataAccess.Entities;
using CashSwiftDeposit.Models;
using CashSwiftDeposit.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashSwiftDeposit.Utils
{
    public static class StringExtentions
    {
        public static string StringReplace(
          this Device Device,
          string template,
          DeviceConfiguration DeviceConfiguration,
          bool isHTML = false)
        {
            List<string> stringList = new List<string>()
      {
        "{Device.name}",
        "{AlertMessageType.title}",
        "{AlertMessageType.description}"
      };
            return null;
        }

        public static string StringReplace(
          this AlertMessageType AlertMessageType,
          string template,
          DeviceConfiguration DeviceConfiguration,
          bool isHTML = false)
        {
            Parallel.ForEach(new List<(string, string)>()
      {
          ($"{{AlertMessageType_name}}", AlertMessageType?.name),
          ($"{{AlertMessageType_title}}", AlertMessageType?.title),
          ($"{{AlertMessageType_description}}", AlertMessageType?.description)
      }, currentToken => template.Replace(currentToken.Item1, currentToken.Item2));
            return template;
        }

        public static string StringReplace(
          this AlertEvent AlertEvent,
          string template,
          DeviceConfiguration DeviceConfiguration,
          bool isHTML = false)
        {
            using (DepositorDBContext depositorDbContext1 = new DepositorDBContext())
            {
                AlertMessageType AlertMessageType = depositorDbContext1.AlertMessageTypes.FirstOrDefault(x => x.id == AlertEvent.alert_type_id);
                template = AlertMessageType != null ? AlertMessageType.StringReplace(template, DeviceConfiguration, isHTML) : null;
                DepositorDBContext depositorDbContext2 = new DepositorDBContext();
                string str1;
                if (depositorDbContext2 == null)
                {
                    str1 = null;
                }
                else
                {
                    var devices = depositorDbContext2.Devices;
                    if (devices == null)
                    {
                        str1 = null;
                    }
                    else
                    {
                        Device Device = devices.FirstOrDefault();
                        str1 = Device != null ? Device.StringReplace(template, DeviceConfiguration, isHTML) : null;
                    }
                }
                template = str1;
                List<(string, string)> source = new List<(string, string)>();
                source.Add(("{AlertEvent_alert_event_id}", AlertEvent?.alert_event_id.ToString().ToUpperInvariant()));
                source.Add(("{AlertEvent_created}", AlertEvent?.created.ToString(DeviceConfiguration.APPLICATION_DATE_FORMAT)));
                source.Add(("{AlertEvent_date_detected}", AlertEvent?.date_detected.ToString(DeviceConfiguration.APPLICATION_DATE_FORMAT)));
                AlertEvent alertEvent1 = AlertEvent;
                DateTime? dateResolved;
                int num;
                if (alertEvent1 == null)
                {
                    num = 0;
                }
                else
                {
                    dateResolved = alertEvent1.date_resolved;
                    num = dateResolved.HasValue ? 1 : 0;
                }
                string str2;
                if (num == 0)
                {
                    str2 = "";
                }
                else
                {
                    AlertEvent alertEvent2 = AlertEvent;
                    if (alertEvent2 == null)
                    {
                        str2 = null;
                    }
                    else
                    {
                        dateResolved = alertEvent2.date_resolved;
                        str2 = dateResolved.Value.ToString(DeviceConfiguration.APPLICATION_DATE_FORMAT);
                    }
                }
                source.Add(("{AlertEvent_date_resolved}", str2));

                Parallel.ForEach(source, currentToken => template.Replace(currentToken.Item1, currentToken.Item2));
                return template;
            }
        }

        public static string CashSwiftReplace(this string s, ApplicationViewModel ApplicationViewModel) => s?.Replace("{transaction_limit_value}", ApplicationViewModel?.CurrentTransaction?.TransactionLimits?.overdeposit_amount.ToString("###,##0.00"))?.Replace("{transaction_underdeposit_amount}", ApplicationViewModel?.CurrentTransaction?.TransactionLimits?.underdeposit_amount.ToString("###,##0.00"))?.Replace("{currency_code}", ApplicationViewModel?.CurrentTransaction?.CurrencyCode?.ToUpper())?.Replace("{bank_name}", ApplicationViewModel?.CurrentSession?.Device?.Branch?.Bank?.name)?.Replace("{branch_name}", ApplicationViewModel?.CurrentSession?.Device?.Branch?.name);
    }
}
