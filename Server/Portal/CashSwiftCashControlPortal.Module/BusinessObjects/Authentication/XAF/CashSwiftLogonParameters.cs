
//BusinessObjects.Authentication.XAF.CashSwiftLogonParameters


using DevExpress.ExpressApp.DC;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Security;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF
{
    [DomainComponent]
    [DisplayName("Log In")]
    [Serializable]
    public class CashSwiftLogonParameters : INotifyPropertyChanged, ISerializable
    {
        private string username;
        private string password;

        public string UserName
        {
            get => username;
            set
            {
                if (username == value)
                    return;
                username = value;
            }
        }

        [PasswordPropertyText(true)]
        public string Password
        {
            get => password;
            set
            {
                if (password == value)
                    return;
                password = value;
            }
        }

        public CashSwiftLogonParameters()
        {
        }

        public CashSwiftLogonParameters(SerializationInfo info, StreamingContext context)
        {
            if (info.MemberCount <= 0)
                return;
            UserName = info.GetString(nameof(UserName));
            Password = info.GetString(nameof(Password));
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged == null)
                return;
            propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [SecurityCritical]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("UserName", UserName);
            info.AddValue("Password", Password);
        }
    }
}
