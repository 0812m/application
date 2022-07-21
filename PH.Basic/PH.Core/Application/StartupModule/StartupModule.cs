using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Core;
using PH.Core.Application.StartupModule;

namespace PH.Core
{
    public abstract class StartupModule:IStartupModule
    {
        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env) { }

        /// <summary>
        /// 配置管道中间件等
        /// </summary>
        /// <param name="services"></param>
        public virtual void ConfigureServices(IServiceCollection services) { }
    }
}
