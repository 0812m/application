using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using PH.Core;
using PH.ToolsLibrary.Json.Converter;
using PH.Web.Core.Authentication;
using PH.Web.Core.Cache;
using PH.Web.Core.Contracts;
using PH.Web.Core.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 添加框架默认服务（控制器、日志、EFCore 仓储模式 + 工作单元、规范化结果中间件）
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddFrameworkService(this IServiceCollection services)
        {
            services.AddControllers(MvcOptionsExtension.Configure())
                .ConfigureIMvcBuilder();

            if (ApplicationContext.ApplicationSetting.UseSwaggerDoc)
                services.AddSwagger();

            services.ConfigureCors();
            services.AddHttpContextAccessor();
            services.AddCacheService();
            return services;
        }

        /// <summary>
        /// 添加缓存服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCacheService(this IServiceCollection services) 
        {
            services.AddDistributedMemoryCache();
            services.AddScoped<ICacheProvide, MemoryCacheProvide>();
            return services;
        }

        /// <summary>
        /// 添加自定义鉴权授权策略
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAppAuthorization(this IServiceCollection services)
        {
            //IAuthorizationPolicyProvider会替换默认的策略提供器
            services.AddSingleton<IAuthorizationPolicyProvider, AppAuthorizationPolicyProvider>();
            services.AddSingleton<IAuthorizationHandler, AppAuthorizationHandler>();
            services.AddTransient<IAuthorizationMiddlewareResultHandler, AppAuthorizationMiddlewareResultHandler>();
            services.AddAuthorization();
            return services;
        }

        /// <summary>
        /// 配置跨域策略
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureCors(this IServiceCollection services) 
        {
            services.AddCors(options => {
                options.AddDefaultPolicy(corsPolicyBuilder => {
                    if (ApplicationContext.Configuration["CorsPolicy"] is null)
                    {
                        corsPolicyBuilder.AllowAnyOrigin();
                        corsPolicyBuilder.AllowAnyHeader();
                        corsPolicyBuilder.AllowAnyMethod();
                    }
                    else
                    {
                       var origins = ApplicationContext.Configuration["CorsPolicy:Origins"];
                        if (string.IsNullOrWhiteSpace(origins))
                            corsPolicyBuilder.AllowAnyOrigin();
                        else 
                        {
                            corsPolicyBuilder.WithOrigins(origins.Split(','));
                        }

                        var header = ApplicationContext.Configuration["CorsPolicy:Header"];
                        if (string.IsNullOrWhiteSpace(header))
                            corsPolicyBuilder.AllowAnyHeader();
                        else
                        {
                            corsPolicyBuilder.WithHeaders(header.Split(','));
                        }

                        var method = ApplicationContext.Configuration["CorsPolicy:Method"];
                        if (string.IsNullOrWhiteSpace(method))
                            corsPolicyBuilder.AllowAnyHeader();
                        else
                        {
                            corsPolicyBuilder.WithMethods(method.Split(','));
                        }
                    }
                });
            });
            return services;
        }

        /// <summary>
        /// 规范化异常处理
        /// </summary>
        /// <param name="app"></param>
        public static void UseSpecificationException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(configure =>
            {
                configure.Run(HandleExceptionAsync);
            });
        }
        private async static Task HandleExceptionAsync(HttpContext httpContext) 
        {
            var feature = httpContext.Features.Get<IExceptionHandlerPathFeature>();
            var result = feature.Error.ExceptionConvertResponse();
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            await httpContext.Response.WriteAsJsonAsync(result);
        }
    }
}
