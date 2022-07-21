using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using Newtonsoft.Json;
using PH.ToolsLibrary.Reflection;

namespace PH.ToolsLibrary.Http
{
    /// <summary>
    /// Request 构建器
    /// </summary>
    public class RequestBuilder
    {
        private Parameter _parameter;
        public RequestBuilder()
        {
            _parameter = new Parameter()
            {
                Headers = new Dictionary<string, string>()
            };
        }

        /// <summary>
        /// 获取默认 Request 构建器
        /// </summary>
        /// <returns></returns>
        public static RequestBuilder DefaultBuilder()
        {
            return new RequestBuilder();
        }

        /// <summary>
        /// 设置Url
        /// </summary>
        /// <param name="url">请求Url</param>
        /// <returns></returns>
        public RequestBuilder SetUrl(string url)
        {
            _parameter.Url = url;
            return this;
        }

        /// <summary>
        /// 添加请求头
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public RequestBuilder AddHeaders(Dictionary<string, string> headers)
        {
            foreach (var item in headers)
            {
                AddHeaders(item.Key, item.Value);
            }
            return this;
        }

        /// <summary>
        /// 添加请求头
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public RequestBuilder AddHeaders(string key, string val)
        {
            _parameter.Headers.TryAdd(key, val);
            return this;
        }

        /// <summary>
        /// 设置请求参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public RequestBuilder SetBody(object param)
        {
            if (param is not null)
                _parameter.Params = param;
            return this;
        }

        /// <summary>
        /// 设置请求方法
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public RequestBuilder UseMethod(HttpMethod method)
        {
            _parameter.Method = method;
            return this;
        }

        /// <summary>
        /// 设置ContentType
        /// </summary>
        /// <returns></returns>
        public RequestBuilder SetContentType(ContentType contentType = ContentType.Json)
        {
            var name = contentType.ToString();
            var targetEnum = contentType.GetType().GetField(name);
            var val = targetEnum.GetCustomAttribute<ValueAttribute>()?.Value;
            _parameter.ContentType = Convert.ToString(val);
            return this;
        }

        /// <summary>
        /// 构建请求
        /// </summary>
        /// <returns></returns>
        public HttpWebRequest Builder()
        {
            if (string.IsNullOrWhiteSpace(_parameter.Url))
                throw new ArgumentNullException("Url 为空");

            //创建请求
            var request = (HttpWebRequest)WebRequest.Create(_parameter.Url);

            //设置请求方式
            if (_parameter.Method is not null)
                request.Method = _parameter.Method.Method;

            //设置Header
            if (_parameter.Headers?.Any() ?? false)
                foreach (var item in _parameter.Headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }

            //设置请求附加参数
            if (_parameter.Params is not null)
            {
                if (_parameter.ContentType == ContentType.Stream.GetAttribute<ValueAttribute>().Value.ToString() && _parameter.Params is Stream)
                {
                    using (var reqStream = request.GetRequestStream())
                    {
                        (_parameter.Params as Stream).CopyTo(reqStream);
                    }
                }
                else
                { 
                    System.Text.Json.JsonSerializer.SerializeAsync(request.GetRequestStream(), _parameter.Params);
                }
            }

            //设置ContentType
            if (string.IsNullOrWhiteSpace(_parameter.ContentType))
                SetContentType();
            request.ContentType = _parameter.ContentType;

            return request;
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> SendAsync<T>()
            where T : class,new()
        {
            var req = Builder();
            try
            {
                var response = await req.GetResponseAsync();
                var result = await response.AsAsync<T>();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback">回调函数</param>
        /// <returns></returns>
        public async Task SendAsync<T>(Action<T> callback)
            where T : class, new()
        {
            try
            {
                var result = await SendAsync<T>();
                callback?.Invoke(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
