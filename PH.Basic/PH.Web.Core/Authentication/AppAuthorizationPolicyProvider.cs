using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.ToolsLibrary.Reflection;
using PH.Web.Core.Authentication;

namespace PH.Web.Core.Authentication
{
    /// <summary>
    /// 策略提供器
    /// </summary>
    public class AppAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        /// <summary>
        /// 默认策略
        /// </summary>
        /// <returns></returns>
        public async Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return await Task.FromResult(new AuthorizationPolicyBuilder().AddAuthenticationSchemes("Bearer").RequireAuthenticatedUser().Build());
        }

        /// <summary>
        /// 备用策略
        /// </summary>
        /// <returns></returns>
        public async Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        {
            return await GetDefaultPolicyAsync();
        }

        /// <summary>
        /// 自定义策略
        /// </summary>
        /// <param name="policyName"></param>
        /// <returns></returns>
        public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName == "AppAuthorization")
            {
               return await Task.FromResult(AppAuthorization());
            }
            return await GetDefaultPolicyAsync();
        }

        /// <summary>
        /// 生成默认的定制策略
        /// </summary>
        /// <returns></returns>
        public AuthorizationPolicy AppAuthorization()
        {
            var appAuthorizationRequirements = new List<AppAuthorizationRequirement?>();
            List<string> authenticationSchemes = new List<string>();

            appAuthorizationRequirements = ReflectionUtil.FindAllImplementClass<IAuthorizationRequirement>().Where(x => !x.IsAbstract && x.IsClass && x.IsPublic)
                 .Select(t =>
                 {
                     var req = Activator.CreateInstance(t) as AppAuthorizationRequirement;
                     return req;
                 }).Where(x => x is not null).ToList();

            if (appAuthorizationRequirements is not null)
            {
                var tmp = appAuthorizationRequirements.Select(x => x.AuthenticationScheme.Split(";"));
                authenticationSchemes.AddRange(tmp.SelectMany(x => x));
            }
            return new AuthorizationPolicy(appAuthorizationRequirements, authenticationSchemes);
        }
    }
}
