// Statuses.XMLSerialization


using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace CashSwift.Library.Standard.Statuses
{
    public static class XMLSerialization
    {
        public static string SerializeToXML<T>(T message, XmlSerializer xmlSerializer)
        {
            string empty = string.Empty;
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = false,
                OmitXmlDeclaration = true,
                NewLineChars = string.Empty,
                NewLineHandling = NewLineHandling.None
            };
            using (StringWriter output = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(output, settings))
                {
                    XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                    namespaces.Add(string.Empty, string.Empty);
                    xmlSerializer.Serialize(xmlWriter, message, namespaces);
                    empty = output.ToString();
                    xmlWriter.Close();
                }
                output.Close();
            }
            return empty;
        }

        public static string SerializeObject<T>(this T toSerialize)
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                new XmlSerializer(toSerialize.GetType()).Serialize(stringWriter, toSerialize);
                return stringWriter.ToString();
            }
        }
    }
}
