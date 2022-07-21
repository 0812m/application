using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PH.Blog.Contract.IServices;
using PH.Blog.Entity;
using PH.Web.Core.Cache;
using PH.Web.Core.Mvc.DynamicWebApi;

namespace PH.Blog.Contract
{
    [NonController]
    public class BaseApiController:IDynamicApi
    {
        protected readonly Identity Identity;
        protected readonly IConfigurtionSvc _configurtionSvc;
        protected readonly ICacheProvide _cache;
        protected IHttpContextAccessor HttpContextAccessor;

        protected HttpContext HttpContext 
        {
            get => HttpContextAccessor.HttpContext;
        }

        public BaseApiController
            (
            IConfigurtionSvc configurtionSvc,
            IHttpContextAccessor httpContextAccessor,
            ICacheProvide cache
            )
        {
            HttpContextAccessor = httpContextAccessor;
            Identity = new Identity(httpContextAccessor);

            _configurtionSvc = configurtionSvc;
            _cache = cache;
        }


        /// <summary>
        /// 获取远程IP
        /// </summary>
        /// <remarks>
        /// X-Forwarded-For 是一个 HTTP 扩展头部，用来表示 HTTP 请求端真实 IP。被各大 HTTP 代理、负载均衡等转发服务广泛使用。
        /// </remarks>
        /// <returns></returns>
        protected string GetRemoteIP() 
        {
            // 获取通过代理访问的ip
            var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            //判断请求是否由 natapp 转发
            if (string.IsNullOrWhiteSpace(ip) || HttpContext.Request.Headers.Any(x => x.Key == "X-Natapp-Ip"))
                ip = HttpContext.Request.Headers["X-Real-Ip"].FirstOrDefault();

            if (string.IsNullOrEmpty(ip)) ip = HttpContext.Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            return ip;
        }

        /// <summary>
        /// 获取请求来源
        /// </summary>
        /// <returns></returns>
        protected string GetSource() 
        {
            HttpContext.Request.Headers.TryGetValue("Source",out var sv);
            return sv.ToString();
        }

        /// <summary>
        /// 生成昵称
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected async Task<(string nickName, string avatar)> GenerateNickName()
        {

           var defaultUsers = await _cache.GetAsync(ConstPool.TYPE_DEFAULT_USER,  () => {
                return  _configurtionSvc.GetByTypeAsync(ConstPool.TYPE_DEFAULT_USER).Result;
            },60 * 120);

            //Random默认根据触发那刻的系统时间做为种子，来产生一个随机数字，如果计算机运行速度很快，如果触发 Randm 函数间隔时间很短，就有可能造成产生一样的随机数
            var random = new Random((int)DateTime.Now.Ticks);
            var user = defaultUsers.ElementAt(random.Next(8));
            return ($"{user.Value}-{random.Next()}", user.SubValue);
        }
    }
}
