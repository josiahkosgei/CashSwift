
// Type: CashSwiftUtil.Licensing.SymmetricKey
// Assembly: CashSwiftUtil, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 885F1C6C-21D2-4135-B89E-154B0975A233
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashSwiftDeposit\App\UI\6.0\CashSwiftUtil.dll

using System;
using System.Xml.Serialization;

namespace CashSwiftUtil.Licensing
{
    [XmlRoot("SK")]
    public struct SymmetricKey
    {
        [XmlElement("K")]
        public byte[] Key;
        [XmlElement("I")]
        public byte[] IV;
        [NonSerialized]
        private static XmlSerializer _serializer = new XmlSerializer(typeof(SymmetricKey));

        public static XmlSerializer Serializer => _serializer;
    }
}
