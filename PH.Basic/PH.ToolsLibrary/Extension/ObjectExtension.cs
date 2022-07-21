using PH.ToolsLibrary.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PH.ToolsLibrary.Extension
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 对象转Xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="writerSettings"></param>
        /// <returns></returns>
        public static string ToXml<T>(this T obj, XmlWriterSettings writerSettings = null)
            where T : class,new()
        {
           return XmlUtil.ObjectToXml(obj, writerSettings);
        }
    }
}
