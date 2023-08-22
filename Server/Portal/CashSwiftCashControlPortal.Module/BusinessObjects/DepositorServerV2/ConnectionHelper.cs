
//BusinessObjects.DepositorServerV2.ConnectionHelper


using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.DepositorServerV2
{
    public static class ConnectionHelper
    {
        public const string ConnectionString = "XpoProvider=MSSqlServer;data source=SQUIRELFIST\\CASHMERESERVER19;integrated security=SSPI;initial catalog=DepositorServer_WIP";

        public static void Connect(AutoCreateOption autoCreateOption)
        {
            XpoDefault.DataLayer = XpoDefault.GetDataLayer("XpoProvider=MSSqlServer;data source=SQUIRELFIST\\CASHMERESERVER19;integrated security=SSPI;initial catalog=DepositorServer_WIP", autoCreateOption);
            XpoDefault.Session =  null;
        }

        public static IDataStore GetConnectionProvider(AutoCreateOption autoCreateOption) => XpoDefault.GetConnectionProvider("XpoProvider=MSSqlServer;data source=SQUIRELFIST\\CASHMERESERVER19;integrated security=SSPI;initial catalog=DepositorServer_WIP", autoCreateOption);

        public static IDataStore GetConnectionProvider(
          AutoCreateOption autoCreateOption,
          out IDisposable[] objectsToDisposeOnDisconnect)
        {
            return XpoDefault.GetConnectionProvider("XpoProvider=MSSqlServer;data source=SQUIRELFIST\\CASHMERESERVER19;integrated security=SSPI;initial catalog=DepositorServer_WIP", autoCreateOption, out objectsToDisposeOnDisconnect);
        }

        public static IDataLayer GetDataLayer(AutoCreateOption autoCreateOption) => XpoDefault.GetDataLayer("XpoProvider=MSSqlServer;data source=SQUIRELFIST\\CASHMERESERVER19;integrated security=SSPI;initial catalog=DepositorServer_WIP", autoCreateOption);
    }
}
