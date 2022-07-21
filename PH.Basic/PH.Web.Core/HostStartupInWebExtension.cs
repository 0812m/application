using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PH.Core.Application.StartupModule;
using PH.Core.Application;
using PH.Core.ConfigurationBuilder;
using PH.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 用于 .Net 6
    /// </summary>
    public static class HostStartupInWebExtension
    {
        /// <summary>
        /// 初始化应用程序
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static WebApplicationBuilder InitApplication(this WebApplicationBuilder builder,string[] args) 
        {
            ApplicationContext.HostingEnvironment = ApplicationContext.WebHostingEnvironment =  builder.Environment;
            
            builder.Host.Configure(args);

            return builder;
        }

        public static void Configure(this ConfigureHostBuilder builder,string[] args)
        {
            builder.ConfigureAppConfiguration((hostContext, configBuilder) =>
            {
                //加载Json文件到配置源
                configBuilder.AddEnvironmentVariables().LoadJsonConfiguration().AddCommandLine(args);

                ApplicationContext.ConfigurationBuilder = configBuilder;
                ApplicationContext.Configuration = ApplicationContext.ConfigurationBuilder.Build();
            });

            builder.ConfigureServices((hostBuilderContext, services) =>
            {
                services.AddConfigurationOption<ApplicationSetting>();
                ApplicationContext.ApplicationSetting = ApplicationContext.Configuration.GetSection("AppSetting").Get<ApplicationSetting>();

                services.Application();

                //配置管道中间件
                services.AddTransient<IStartupFilter, StartupFilter>();
            });
        }
    }
}
