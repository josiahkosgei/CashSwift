// Utils.AlertClasses.AlertBase


using CashSwiftDataAccess.Data;
using CashSwiftDataAccess.Entities;
using System;
using System.Collections.Generic;

namespace CashSwiftDeposit.Utils.AlertClasses
{
    public class AlertBase : IAlert
    {
        private Device _device;
        private AlertMessageType _alertType;
        private DateTime _dateDetected = DateTime.Now;
        private DateTime _dateResolved;
        private IDictionary<string, string> _tokens;

        public Device Device
        {
            get => _device;
            set => _device = value;
        }

        public AlertMessageType AlertType
        {
            get => _alertType;
            set => _alertType = value;
        }

        public DateTime DateDetected
        {
            get => _dateDetected;
            set => _dateDetected = value;
        }

        public DateTime DateResolved
        {
            get => _dateResolved;
            set => _dateResolved = value;
        }

        protected IDictionary<string, string> Tokens
        {
            get => _tokens;
            set => _tokens = value;
        }

        public AlertBase(Device device, DateTime dateDetected)
        {
            Device = device;
            DateDetected = dateDetected;
        }

        protected virtual AlertEmail GetAlertEmail(DepositorDBContext DBContext) => throw new NotImplementedException();

        protected string GetHTMLBody() => throw new NotImplementedException();

        protected virtual string GetRawTextBody() => throw new NotImplementedException();

        protected virtual string GetSMSBody() => throw new NotImplementedException();

        protected virtual void GenerateTokens() => throw new NotImplementedException();

        protected virtual string GenerateHTMLMessageToken() => throw new NotImplementedException();

        protected virtual string GenerateRawTextMessageToken() => throw new NotImplementedException();

        protected virtual string GenerateSMSMessageToken() => throw new NotImplementedException();

        protected virtual AlertEvent GetCorrespondingAlertEvent(DepositorDBContext DBContext) => throw new NotImplementedException();

        protected virtual AlertEmail GenerateEmail(DepositorDBContext DBContext) => throw new NotImplementedException();

        protected virtual AlertEmail GenerateSMS() => throw new NotImplementedException();

        public virtual bool SendAlert() => throw new NotImplementedException();
    }
}
