// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.CoopMessageBase
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses
{
    public class CoopMessageBase
    {
        [NonSerialized]
        private static XmlSerializer _serializer = new XmlSerializer(typeof(CoopMessageBase));

        [XmlIgnore]
        public string RawXML { get; internal set; }

        public static XmlSerializer Serializer => _serializer;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}