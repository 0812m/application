using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PH.ToolsLibrary.Http
{
    internal class Parameter
    {
        /// <summary>
        /// Url
        /// </summary>
        internal string Url { get; set; }

        /// <summary>
        /// 请求方法
        /// </summary>
        internal HttpMethod Method { get; set; }

        /// <summary>
        /// 请求头
        /// </summary>
        internal Dictionary<string,string> Headers { get; set; }

        /// <summary>
        /// ContentType
        /// </summary>
        internal string ContentType { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        internal object Params { get; set; }
    }
}
