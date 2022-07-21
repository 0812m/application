using PH.ToolsLibrary.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using PH.ToolsLibrary.Reflection;
using PH.ToolsLibrary;

namespace PH.Aliyun.Service
{
    /// <summary>
    /// 详细查阅： https://help.aliyun.com/document_detail/410750.html
    /// </summary>
    [Obsolete("验证签名不通过，暂不使用",true)]
    public class OSSUtil
    {
        private static OSSOptions options;
        static OSSUtil()
        {
            options = new OSSOptions();
        }

        /// <summary>
        /// 创建签名
        /// </summary>
        /// <param name="method"></param>
        /// <param name="contentType"></param>
        /// <param name="date"></param>
        /// <param name="canonicalizedResource"></param>
        /// <returns></returns>
        private static string CreateSignature(string method, string contentType, string time, string canonicalizedResource)
        {
            //Signature = base64(hmac - sha1(AccessKeySecret,
            //VERB + "\n"
            //+ Content - MD5 + "\n"
            //+ Content - Type + "\n"
            //+ Date + "\n"
            //+ CanonicalizedOSSHeaders
            //+ CanonicalizedResource))
            string data = $"{method}\n{contentType}\n{time}\n{canonicalizedResource}";
            var res = aa(options.Secret, data); //SecurityUtil.HMACS_HA1Encrypt(options.Secret, data, Encoding.UTF8).ToLower();
            return res.EncodeBase64(Encoding.UTF8);
        }

        public static string aa(string key ,string data) 
        {
            using (var algorithm = KeyedHashAlgorithm.Create("HmacSHA1".ToUpperInvariant()))
            {
                algorithm.Key = Encoding.UTF8.GetBytes(key.ToCharArray());
                return Convert.ToBase64String(
                    algorithm.ComputeHash(Encoding.UTF8.GetBytes(data.ToCharArray())));
            }
        }

        /// <summary>
        /// Authorization
        /// </summary>
        /// <param name="method"></param>
        /// <param name="contentType"></param>
        /// <param name="canonicalizedResource"></param>
        /// <returns></returns>
        private static string CreateAuthorization(string method, string time,string contentType, string canonicalizedResource)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("OSS ");
            sb.Append(options.AccessKey);
            sb.Append(":");
            var Signature = CreateSignature(method, contentType, time, canonicalizedResource);
            sb.Append(Signature);
           var a = sb.ToString();
            return a;
        }

        public static RequestBuilder CreateRequest(string canonicalizedResource, HttpMethod method, ContentType contentType)
        {
            var now = DateTimeOffset.Now.ToString("r");
            var resource = $"x-oss-meta-magic:{canonicalizedResource}";
            var _contentType = contentType.GetAttribute<ValueAttribute>()?.Value?.ToString();
            return RequestBuilder.DefaultBuilder()
                 .SetUrl($"{options.Endpoint}/{canonicalizedResource}")
                 .UseMethod(method)
                 .SetContentType(contentType)
                 .AddHeaders("Authorization", CreateAuthorization(method.Method, now, _contentType, canonicalizedResource));
        }
    }
}
