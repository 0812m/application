using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PH.Blog.Contract;
using PH.Blog.Contract.IServices;
using PH.Web.Core.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Web.Core
{
    public class SecurityDefinitionRequirement : AppAuthorizationRequirement
    {
        public override async Task<bool> HandleAsync(HttpContext httpContext)
        {
            //未定义安全特性则通过。
            var securityDefinition = httpContext.GetEndpoint()?.Metadata?.GetMetadata<SecurityDefinitionAttribute>();
            if (securityDefinition is null) return true;

            //未拥有角色，不予通过。
            var roeJoin = httpContext.User?.Claims?.FirstOrDefault(x => x.Type.Equals(nameof(Identity.RoleJoin)))?.Value;
            if (string.IsNullOrWhiteSpace(roeJoin)) return false;

            var svc = httpContext.RequestServices.GetService<IAuthenticationSvc>();
            var privileges = await svc.GetPrivilegeByRoleCodeAsync(roeJoin.Split(','));

            return privileges.Contains(securityDefinition.Permission);
        }
    }
}
