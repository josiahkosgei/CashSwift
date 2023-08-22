using System.Xml.Serialization;
using System.Xml;
using System.Dynamic;
using System.Xml.Linq;

namespace CashSwift.Finacle.Integration.Extensions
{
    public class XmlWrapper
    {
        public static dynamic Convert(XElement parent)
        {
            dynamic output = new ExpandoObject();

            output.Name = parent.Name.LocalName;
            output.Value = parent.Value;

            output.HasAttributes = parent.HasAttributes;
            if (parent.HasAttributes)
            {
                output.Attributes = new List<KeyValuePair<string, string>>();
                foreach (XAttribute attr in parent.Attributes())
                {
                    KeyValuePair<string, string> temp = new KeyValuePair<string, string>(attr.Name.LocalName, attr.Value);
                    output.Attributes.Add(temp);
                }
            }

            output.HasElements = parent.HasElements;
            if (parent.HasElements)
            {
                output.Elements = new List<dynamic>();
                foreach (XElement element in parent.Elements())
                {
                    dynamic temp = Convert(element);
                    output.Elements.Add(temp);
                }
            }

            return output;
        }
    }
    public static partial class XmlExtensions
    {
        public static T DeserializeXMLFileToObject<T>(string XmlFilename)
        {
            T returnObject = default(T);
            if (string.IsNullOrEmpty(XmlFilename)) return default(T);

            try
            {
                StreamReader xmlStream = new StreamReader(XmlFilename);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                returnObject = (T)serializer.Deserialize(xmlStream);
            }
            catch (Exception ex)
            {
                // ExceptionLogger.WriteExceptionToConsole(ex, DateTime.Now);
            }
            return returnObject;
        }
        public static T GetObjectFromXml<T>(string path, string localName, string namespaceURI, bool ignoreNamespaces = false)
        {
            using (var textReader = new StreamReader(path))
                return GetObjectFromXml<T>(textReader, localName, namespaceURI);
        }

        public static T GetObjectFromXml<T>(TextReader textReader, string localName, string namespaceURI, bool ignoreNamespaces = false)
        {
            using (var xmlReader = ignoreNamespaces ? new NamespaceIgnorantXmlTextReader(textReader) : XmlReader.Create(textReader))
                return GetObjectFromXml<T>(xmlReader, localName, namespaceURI);
        }

        public static T GetObjectFromXml<T>(XmlReader reader, string localName, string namespaceURI)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "IWantThis" && reader.NamespaceURI == namespaceURI)
                {
                    var serializer = XmlSerializerFactory.Create(typeof(T), localName, namespaceURI);
                    using (var subReader = reader.ReadSubtree())
                        return (T)serializer.Deserialize(subReader);
                }
            }
            // Or throw an exception?
            return default(T);
        }
    }

    // This class copied from this answer https://stackoverflow.com/a/873281/3744182
    // To https://stackoverflow.com/questions/870293/can-i-make-xmlserializer-ignore-the-namespace-on-deserialization
    // By https://stackoverflow.com/users/48082/cheeso
    // helper class to ignore namespaces when de-serializing
    public class NamespaceIgnorantXmlTextReader : XmlTextReader
    {
        public NamespaceIgnorantXmlTextReader(TextReader reader) : base(reader) { }

        public override string NamespaceURI { get { return ""; } }
    }

    public static class XmlSerializerFactory
    {
        // To avoid a memory leak the serializer must be cached.
        // https://stackoverflow.com/questions/23897145/memory-leak-using-streamreader-and-xmlserializer
        // This factory taken from 
        // https://stackoverflow.com/questions/34128757/wrap-properties-with-cdata-section-xml-serialization-c-sharp/34138648#34138648

        readonly static Dictionary<Tuple<Type, string, string>, XmlSerializer> cache;
        readonly static object padlock;

        static XmlSerializerFactory()
        {
            padlock = new object();
            cache = new Dictionary<Tuple<Type, string, string>, XmlSerializer>();
        }

        public static XmlSerializer Create(Type serializedType, string rootName, string rootNamespace)
        {
            if (serializedType == null)
                throw new ArgumentNullException();
            if (rootName == null && rootNamespace == null)
                return new XmlSerializer(serializedType);
            lock (padlock)
            {
                XmlSerializer serializer;
                var key = Tuple.Create(serializedType, rootName, rootNamespace);
                if (!cache.TryGetValue(key, out serializer))
                {
                    cache[key] = serializer = new XmlSerializer(serializedType, new XmlRootAttribute { ElementName = rootName, Namespace = rootNamespace });
                }
                return serializer;
            }
        }
    }

}
