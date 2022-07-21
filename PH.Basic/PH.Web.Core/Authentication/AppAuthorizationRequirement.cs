using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Web.Core.Authentication
{
    /// <summary>
    /// 系统策略
    /// </summary>
    public abstract  class AppAuthorizationRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// “;”分割支持多个
        /// </summary>
        public string AuthenticationScheme = "Bearer";

        /// <summary>
        /// 失败原因
        /// </summary>
        public string Message;

        public abstract Task<bool> HandleAsync(HttpContext httpContext);
    }
}
