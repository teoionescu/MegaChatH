using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CommonLibrary
{
    public class Serializer
    {
        private static Serializer instance;

        private static Serializer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Serializer();
                }
                return instance;
            }
        }

        private Type[] derivedMessages;

        private Serializer()
        {
            var derivedType = typeof(MessageBase);
            var assembly = Assembly.GetAssembly(derivedType);
            derivedMessages = assembly.GetTypes().Where(t => t != derivedType && derivedType.IsAssignableFrom(t)).ToArray();
        }

        private string SerializeToXml(MessageBase objectToSerialize)
        {
            StringBuilder sb = new StringBuilder();

            XmlWriterSettings settings = new XmlWriterSettings { /*Indent = true,*/ OmitXmlDeclaration = true};

            using (XmlWriter xmlWriter = XmlWriter.Create(sb, settings))
            {
                if (xmlWriter != null)
                {
                    var ser = new XmlSerializer(typeof(MessageBase),derivedMessages);
                    ser.Serialize(xmlWriter, objectToSerialize);
                }
            }

            return sb.ToString();
        }

        private MessageBase DeserializeFromXml(string xmlString)
        {
            XmlSerializer xs = new XmlSerializer(typeof(MessageBase),derivedMessages);
            using (MemoryStream memoryStream = new MemoryStream(xmlString.ToCharArray().Select(c => (byte)c).ToArray()))
            {
                return xs.Deserialize(memoryStream) as MessageBase;
            }
        }

        public static string Serialize(MessageBase st)
        {
            return Instance.SerializeToXml(st);
        }

        public static MessageBase Deserialize(string st)
        {
            return Instance.DeserializeFromXml(st);
        }
    }
}