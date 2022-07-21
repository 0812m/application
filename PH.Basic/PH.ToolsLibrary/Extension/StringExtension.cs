using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.ToolsLibrary.Http;
using PH.ToolsLibrary.Xml;

namespace PH.ToolsLibrary.Extension
{
    public static class StringExtension
    {
        /// <summary>
        /// 发送 Get 请求并返回响应结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(this string url) 
            where T : class,new()
        {
           return await RequestBuilder.DefaultBuilder()
                .SetUrl(url)
                .UseMethod(System.Net.Http.HttpMethod.Get)
                .SendAsync<T>();
        }

        /// <summary>
        /// xml 字符串转 Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T XmlToModel<T>(this string xml)
        {
           return XmlUtil.XmlToObject<T>(xml);
        }
    }
}
