using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PH.Authentication.JwtBearer;
using PH.Authentication.JwtBearer.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class JwtBearerExtension
    {
        /// <summary>
        /// 添加 JwtBearer 授权服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="jwtSetting">jwt配置</param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddJwtBearerService<T>(this IServiceCollection services,IConfiguration configuration = null, Action<JwtBearerOptions> action = null) 
            where T : JwtSetting
        {
            if (configuration is null)
            {
                var factory = services.FirstOrDefault(x => x.ServiceType == typeof(IConfiguration))?.ImplementationFactory;
                configuration = factory?.Invoke(null) as IConfiguration;
            }
             
            var JwtSettingConfiguration = configuration.GetSection("JwtSetting");
            if (JwtSettingConfiguration is null)
                throw new ArgumentNullException();

            services.Configure<T>(JwtSettingConfiguration);
            services.Configure<JwtSetting>(JwtSettingConfiguration);
            var jwtSetting = JwtSettingConfiguration.Get<T>();

            if (action is null)
                action = options =>
                {
                    options.TokenValidationParameters = jwtSetting.TokenValidationParameters ?? new IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey =  jwtSetting.ValidateIssuerSigningKey,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecurityKey)),
                        ValidateIssuer = jwtSetting.ValidateIssuer,
                        ValidIssuer = jwtSetting.Issuer,
                        ValidateAudience = jwtSetting.ValidateAudience,
                        ValidAudience = jwtSetting.Audience,
                        ValidateLifetime = jwtSetting.ValidateLifetime,
                        ClockSkew = TimeSpan.FromMinutes(jwtSetting.ClockSkew)
                    };
                    options.Events = new JwtBearerEvents();
                    options.Events.OnChallenge = jwtSetting.OnChallenge;
                    options.Events.OnForbidden = jwtSetting.OnForbidden;
                    options.Events.OnAuthenticationFailed = jwtSetting.OnAuthenticationFailed;
                };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(action);
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            return services;
        }
    }
}
