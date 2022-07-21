using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PH.Authentication.JwtBearer.Options
{
    public class JwtSetting
    {
        /// <summary>
        /// 加密算法，对应 SecurityAlgorithms 类中的算法
        /// </summary>
        public virtual string Algorithms { get; set; }

        /// <summary>
        /// 是否验证密钥
        /// </summary>
        public virtual bool ValidateIssuerSigningKey { get; set; }

        /// <summary>
        /// 签发方密钥
        /// </summary>
        public virtual string SecurityKey { get; set; }

        /// <summary>
        /// 是否验证签发方
        /// </summary>
        public virtual bool ValidateIssuer { get; set; }

        /// <summary>
        /// 签发方
        /// </summary>
        public virtual string Issuer { get; set; }

        /// <summary>
        /// 是否验证接收者
        /// </summary>
        public virtual bool ValidateAudience { get; set; }

        /// <summary>
        /// 接收者
        /// </summary>
        public virtual string Audience { get; set; }

        /// <summary>
        /// 是否验证 token 过期时间
        /// </summary>
        public virtual bool ValidateLifetime { get; set; }

        /// <summary>
        /// 过期时间（分钟）
        /// </summary>
        public virtual long ExpiredTime { get; set; }

        /// <summary>
        /// 令牌过期时间的偏移值，缓冲过期时间，总有效时间 = 过期时间 + 缓存时间，不设置的话默认是 5 分钟
        /// </summary>
        public virtual long ClockSkew { get; set; }

        /// <summary>
        /// token  验证参数
        /// </summary>
        public virtual TokenValidationParameters TokenValidationParameters { get; set; }

        public JwtSetting()
        {

        }

        /// <summary>
        /// 身份质疑，如无效token
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual Task OnChallenge(JwtBearerChallengeContext context)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 权限验证失败
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual Task OnForbidden(ForbiddenContext context)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// token 过期
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual Task OnAuthenticationFailed(AuthenticationFailedContext context)
        {
            //Token 过期 
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }
            return Task.CompletedTask;
        }
    }
}
