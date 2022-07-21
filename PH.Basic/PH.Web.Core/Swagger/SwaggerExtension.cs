using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;
using PH.Core;
using PH.Web.Core;
using PH.Web.Core.Mvc;
using PH.Web.Core.Mvc.DynamicWebApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.ConfiguraSwaggerDoc();

                options.ConfiguraSecurityDefinition();

                //controller注释;必须放最后,否则后面的会覆盖前面的
                options.ConfiguraXmlComments();
            });

            return services;
        }

        /// <summary>
        /// 添加安全认证
        /// </summary>
        /// <param name="options"></param>
        public static void ConfiguraSecurityDefinition(this SwaggerGenOptions options) 
        {
            //定义JwtBearer认证
            options.AddSecurityDefinition("JwtBearer", new OpenApiSecurityScheme()
            {
                Description = "JwtBearer 认证(直接在输入框中输入认证信息，不需要在开头添加Bearer)",
                Name = "Authorization",//jwt默认的参数名称
                In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });

            //声明一个Scheme，注意下面的Id要和上面AddSecurityDefinition中的参数name一致
            var scheme = new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference() { Type = ReferenceType.SecurityScheme, Id = "JwtBearer" }
            };

            //注册全局认证（所有的接口都可以使用认证）
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                [scheme] = new string[0]
            });

        }

        /// <summary>
        /// 配置 API 文档
        /// </summary>
        /// <param name="options"></param>
        private static void ConfiguraSwaggerDoc(this SwaggerGenOptions options)
        {
            options.SwaggerDoc("default", new OpenApiInfo
            {
                Version = "",
                Title = "phqmo.com API 管理中心",
                Description = "接口说明(多模式管理,右上角切换)"
                //Contact = new OpenApiContact { Name = "www.phqmo.com", Email = "321304825@qq.com" }
            });

            foreach (var controller in ApplicationContext.Controllers)
            {
                var name = controller.Key;
                var apiDescriptionSettings = controller.Value.GetCustomAttribute<ApiDescriptionSettingsAttribute>();
                options.SwaggerDoc(name, new OpenApiInfo() 
                {
                    Version = apiDescriptionSettings?.Version,
                     Title = apiDescriptionSettings?.GroupName,
                     Description = apiDescriptionSettings?.Description,
                     Contact = new OpenApiContact { Name = apiDescriptionSettings?.Contact, Email = apiDescriptionSettings?.Email }
                });
            }

            options.DocInclusionPredicate((docName, apiDesc) => 
            {
                var controllerActionDescriptor = apiDesc.ActionDescriptor as ControllerActionDescriptor;
                return docName == controllerActionDescriptor?.ControllerName;
            });
        }

        /// <summary>
        /// 配置注释文档
        /// </summary>
        /// <param name="options"></param>
        private static void ConfiguraXmlComments(this SwaggerGenOptions options)
        {
            var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, $"*.xml", SearchOption.TopDirectoryOnly);
            // 启用xml注释
            foreach (var item in xmlFiles)
            {
                options.IncludeXmlComments(item, true);
            }
        }

        /// <summary>
        /// 使用 Swagger 
        /// </summary>
        /// <param name="app"></param>
        public static void UseFrameworkSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //获取所有已注册的 controller
                var applicationPartManager = app.ApplicationServices.GetService<ApplicationPartManager>();
                var controllerFeature = new ControllerFeature();
                applicationPartManager.PopulateFeature(controllerFeature);

                //添加默认模块
                c.SwaggerEndpoint($"/swagger/default/swagger.json", "下拉选择模块");
                //遍历已注册的 controller
                foreach (var controller in controllerFeature.Controllers)
                {
                    var name = MvcCommon.TrimControllerNameEnd(controller.Name);
                    //保存起来 AddSwagger 用
                    ApplicationContext.Controllers.TryAdd(name,controller);

                    var apiDescriptionSettings = controller.GetCustomAttribute<ApiDescriptionSettingsAttribute>();
                    c.SwaggerEndpoint($"/swagger/{name}/swagger.json", apiDescriptionSettings?.GroupName);
                }
            });
        }
    }
}
