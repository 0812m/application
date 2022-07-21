using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace PH.ToolsLibrary.Xml
{
    public static class XmlUtil
    {
        /// <summary>
        /// Xml 字符串转 T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T XmlToObject<T>(string xml, XmlReaderSettings xmlReaderSettings = null) 
        {
            T obj = default(T);
            //if (!xml.StartsWith("<?xml"))
            //    xml = @"<?xml version=""1.0"" encoding=""utf-8""?>" + xml;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(xml);
                using (MemoryStream ms = new MemoryStream(buffer))
                    obj = (T)xmlSerializer.Deserialize(ms);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return obj;
        }

        /// <summary>
        /// T 转 Xml
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToXml(object obj,XmlWriterSettings xmlWriterSettings = null) 
        {
            var xmlString = string.Empty;

            xmlWriterSettings = xmlWriterSettings ?? DefaultXmlWriterSettings();
            using (MemoryStream ms = new MemoryStream())
            {
                //去除默认命名空间xmlns:xsd和xmlns:xsi
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                XmlSerializer formatter = new XmlSerializer(obj.GetType());
                formatter.Serialize(ms, obj, ns);
                xmlString = Encoding.Default.GetString(ms.ToArray());
            }
            return xmlString;
        }

        public static XmlReaderSettings DefaultXmlReaderSettings() 
        {
           var xmlReaderSettings = new XmlReaderSettings();
            return xmlReaderSettings;
        }

        public static XmlWriterSettings DefaultXmlWriterSettings() 
        {
            var xmlWriterSettings = new XmlWriterSettings();
            return xmlWriterSettings;
        }
    }
}
