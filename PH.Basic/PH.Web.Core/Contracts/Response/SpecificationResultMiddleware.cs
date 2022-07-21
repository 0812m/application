using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace PH.Web.Core.Contracts
{
    /// <summary>
    /// 规范化结果中间件
    /// </summary>
    public class SpecificationResultMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            //由于Asp.Net Core 默认不允许修改响应 故先保留原始响应流
            var originalResponseStream = context.Response.Body;

            using (var ms =new MemoryStream())
            {
                context.Response.Body = ms;

                //执行下一个中间件
                await next(context);

                Response response = new Response();

                if (string.IsNullOrWhiteSpace(context.Response.ContentType) || context.Response.ContentType.StartsWith("application/json") )
                {
                    response.Data = System.Text.Json.JsonSerializer.Deserialize<object>(context.Response.Body);
                    ReplaceBodyStreamAsync(context.Response, originalResponseStream, response);
                }
            }
        }

        /// <summary>
        /// 替换响应流
        /// </summary>
        /// <param name="response"></param>
        /// <param name="ms"></param>
        /// <param name="originalResponseStream"></param>
        /// <param name="requestLogContext"></param>
        /// <returns></returns>
        private  void ReplaceBodyStreamAsync(HttpResponse response ,Stream originalResponseStream, Response responseObj) 
        {
            BinaryFormatter binary = new BinaryFormatter();
            binary.Serialize(originalResponseStream, responseObj);
            response.Body = originalResponseStream;
        }
    }
}
