using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PH.Web.Core.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Web.Core.Authentication
{
    /// <summary>
    /// 自定义授权处理器
    /// </summary>
    public class AppAuthorizationHandler : IAuthorizationHandler
    {
        public virtual async Task HandleAsync(AuthorizationHandlerContext context)
        {
            var httpContext = context.Resource as DefaultHttpContext;
            foreach (var requirement in context.Requirements)
            {
                var appRequirement = requirement as AppAuthorizationRequirement;
                var isSuccess = await appRequirement.HandleAsync(httpContext);
                if (isSuccess)
                    context.Succeed(requirement);
                else
                    context.Fail(new AuthorizationFailureReason(this, appRequirement.Message));
            }
        }
    }
}
