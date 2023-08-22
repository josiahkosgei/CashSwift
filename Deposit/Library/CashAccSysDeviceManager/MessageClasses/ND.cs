// Decompiled with JetBrains decompiler
// Type: ND
// Assembly: CashAccSysDeviceManager, Version=6.2.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 3301DEC8-4781-402A-85F0-62E202788F00
// Assembly location: C:\DEV\maniwa\bak\New folder\6.0 - Demo\CashAccSysDeviceManager.dll

using System.Collections.Generic;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "ND")]
    public class ND
    {
        [XmlElement(ElementName = "Cassette")]
        public List<Cassette> Cassette { get; set; }

        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "DeviceID")]
        public string DeviceID { get; set; }

        [XmlAttribute(AttributeName = "Status")]
        public string Status { get; set; }

        [XmlAttribute(AttributeName = "CassetteState")]
        public string CassetteState { get; set; }

        [XmlAttribute(AttributeName = "DispenseCurrency")]
        public string DispenseCurrency { get; set; }
    }
}
