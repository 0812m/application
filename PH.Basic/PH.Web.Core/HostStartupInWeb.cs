using Microsoft.AspNetCore.Hosting;
using PH.Core.Application.StartupModule;
using PH.Core.Application;
using PH.Core.ConfigurableOptions;
using PH.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PH.Core.ConfigurationBuilder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

[assembly: HostingStartup(typeof(PH.Web.Core.HostStartupInWeb))]
namespace PH.Web.Core
{
    /// <summary>
    /// 用于.Net 5 及之前
    /// </summary>
    public class HostStartupInWeb : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((hostContext, configBuilder) =>
                {
                    ApplicationContext.HostingEnvironment = ApplicationContext.WebHostingEnvironment = hostContext.HostingEnvironment;

                    //加载Json文件到配置源
                    configBuilder.LoadJsonConfiguration();

                    ApplicationContext.ConfigurationBuilder = configBuilder;
                    ApplicationContext.Configuration = ApplicationContext.ConfigurationBuilder.Build();
                }).ConfigureServices(services =>
                {
                    services.AddConfigurationOption<ApplicationSetting>();
                    ApplicationContext.ApplicationSetting = ApplicationContext.Configuration.GetSection("AppSetting").Get<ApplicationSetting>();

                    //启用自动注入
                    services.Application();

                    //配置管道中间件
                    services.AddTransient<IStartupFilter, StartupFilter>();
                });
        }
    }
}
