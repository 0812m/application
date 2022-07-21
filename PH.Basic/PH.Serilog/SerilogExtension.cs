using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Events;
using Serilog.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SerilogExtension
{
        public static IServiceCollection AddSerilog(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddLogging(builder => 
            {
                var loggerConfiguration = new LoggerConfiguration();
                loggerConfiguration.ReadFrom.Configuration(configuration, "Logging");
                builder.AddSerilog(loggerConfiguration.CreateLogger());
            });
            
            return services;
        }

        public static IServiceCollection AddFrameworkSerilog(this IServiceCollection services)
        {
            var factory = services.FirstOrDefault(x => x.ServiceType == typeof(IConfiguration)).ImplementationFactory;
                services.AddSerilog(factory.Invoke(null) as IConfiguration);
            
            return services;
        }

        /// <summary>
        /// .Net 6 用
        /// </summary>
        /// <param name="webApplicationBuilder"></param>
        /// <param name="configAction"></param>
        /// <returns></returns>
        public static WebApplicationBuilder UseDefaultSerilog(this WebApplicationBuilder webApplicationBuilder, Action<LoggerConfiguration> configAction = null) 
        {
            webApplicationBuilder.Host.UseSerilog((context, configuration) => 
            {
                // 加载配置文件
                var config = configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext();

                if (configAction != null) configAction.Invoke(config);
                else
                {
                    config.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
                          .WriteTo.File(Path.Combine("logs", "app_.log"), LogEventLevel.Information, rollingInterval: RollingInterval.Day, retainedFileCountLimit: null, encoding: Encoding.UTF8);
                }
            });
            return webApplicationBuilder;
        }
    }
}
