
//Properties.Settings


using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CashSwiftCashControlPortal.Module.Properties
{
  [CompilerGenerated]
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.9.0.0")]
  internal sealed class Settings : ApplicationSettingsBase
  {
    private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

    public static Settings Default => Settings.defaultInstance;

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("False")]
    public bool SHOW_ERRORS_IN_PORTAL => (bool) this[nameof (SHOW_ERRORS_IN_PORTAL)];

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("900")]
    public int MessageKeepAliveTime => (int) this[nameof (MessageKeepAliveTime)];
  }
}
