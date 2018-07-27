using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Auth.Infrastructure.Utility
{
    /// <summary>
    /// Xml工具
    /// </summary>
    public class XmlUtils
    {
        /// <summary>
        /// XML反序列化
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(string xml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StringReader stringReader = new StringReader(xml))
            {
                return (T)xmlSerializer.Deserialize(stringReader);
            }
        }
        /// <summary>
        /// XML序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string XmlSerialize<T>(T obj)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (MemoryStream memoryStream = new MemoryStream())
            {
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, new UTF8Encoding(false))
                {
                    Formatting = Formatting.Indented
                };
                xmlSerializer.Serialize(xmlTextWriter, obj);
                xmlTextWriter.Flush();
                xmlTextWriter.Close();
                var uTf8Encoding = new UTF8Encoding(false, true);
                return uTf8Encoding.GetString(memoryStream.ToArray());
            }
        }
    }
}
