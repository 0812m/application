using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using PH.Web.Core.Authentication;
using PH.Web.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Web.Core.Authentication
{
    /// <summary>
    /// 用于针对授权结果，进行不同的响应处理
    /// </summary>
    public class AppAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            Response response = null;
            if (authorizeResult.Challenged)
            {
                response = new Response()
                {
                    Code = 401,
                    Message = "Unauthorized"
                };
            }
            else if (authorizeResult.Forbidden || authorizeResult.AuthorizationFailure is not null)
            {
                IEnumerable<string> reasons = null;
                if (authorizeResult.AuthorizationFailure is not null)
                {
                    var message = authorizeResult.AuthorizationFailure.FailureReasons.Select(x => x.Message).ToList();
                    if (message.Any(x => !string.IsNullOrWhiteSpace(x)))
                        reasons = message;
                }
                response = new Response()
                {
                    Code = 403,
                    Message = "Forbidden",
                    Data = reasons
                };
            }
            if (response is not null)
            {
                await context.Response.WriteAsJsonAsync(response);
            }
            else
                await next(context);
        }
    }
}
