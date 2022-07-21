using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PH.Blog.Contract.IServices;
using PH.Blog.Entity;
using PH.Core.IOC.Attributes;
using PH.Core.IOC.Enum;

namespace PH.Blog.Contract
{
    /// <summary>
    /// 用户身份模型
    /// </summary>
    [AutoInjection(patterns: InjectionPatterns.Self)]
    public class Identity
    {
        private readonly IServiceProvider _serviceProvider;
        public Identity()
        {

        }

        public Identity(IHttpContextAccessor httpContextAccessor)
        {
            var httpcontext = httpContextAccessor.HttpContext;
            _serviceProvider = httpContextAccessor.HttpContext.RequestServices;

            var claims = httpcontext.User?.Claims;
            if (claims is null || !claims.Any()) return;
            int.TryParse(claims.FirstOrDefault(x => x.Type.Equals(nameof(UserId)))?.Value, out var userId);
            UserId = userId;
            NickName = claims.FirstOrDefault(x => x.Type.Equals(nameof(NickName)))?.Value;
            Description = claims.FirstOrDefault(x => x.Type.Equals(nameof(Description)))?.Value;
            DateTime.TryParse(claims.FirstOrDefault(x => x.Type.Equals(nameof(Description)))?.Value, out var createTime);
            CreateTime = createTime;
            RoleJoin = claims.FirstOrDefault(x => x.Type.Equals(nameof(RoleJoin)))?.Value;
        }

        public virtual int UserId { get; set; }

        public virtual string NickName { get; set; }

        public virtual string Description { get; set; }

        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 以 "," 分隔存储Code
        /// </summary>
        public virtual string RoleJoin { get; set; }

        public IEnumerable<string> GetRoles()
        {
            if (string.IsNullOrWhiteSpace(RoleJoin))
                return new List<string>();
            return RoleJoin.Split(',');
        }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        public async Task<User> CurrentUserAsync()
        {
            var svc = _serviceProvider.GetService<IAuthenticationSvc>();
            return await svc.GetAsync(UserId);
        }
    }
}
