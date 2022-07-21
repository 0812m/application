using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PH.Core.Application.StartupModule
{
    public class StartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app => 
            {
                ApplicationContext.ApplicationServices = app.ApplicationServices;

                UseStartup(app);

                next(app);
            };
        }

        /// <summary>
        /// 执行 IStartupModule 中的 Configure 方法
        /// </summary>
        /// <param name="app"></param>
        public void UseStartup(IApplicationBuilder app) 
        {
            foreach (var startup in ApplicationContext.AppStartups)
            {
                //从当前类型中找出 Name = Configure && Return = void && 第一个参数为 IApplicationBuilder 的认为是管道配置方法
               var methods = startup.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(x => x.Name == nameof(IStartupModule.Configure) 
                    && x.ReturnType == typeof(void)  
                    && x.GetParameters().Length > 0
                    && x.GetParameters().First().ParameterType == typeof(IApplicationBuilder));

                if (!methods.Any()) continue;

                foreach (var method in methods)
                {
                    method.Invoke(startup, GetParameterByIServiceProvider(method,app));
                }
            }
        }

        /// <summary>
        /// 从全局的 IServiceProvider 中获取参数
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public object[] GetParameterByIServiceProvider(MethodInfo method,IApplicationBuilder app) 
        {
            var paramters = method.GetParameters();
            var objs = new object[paramters.Length];
            objs[0] = app;
            for (int i = 1; i < paramters.Length; i++)
            {
                objs[i] = ApplicationContext.ApplicationServices.GetService(paramters[i].ParameterType);
            }
            return objs;
        }
    }
}
