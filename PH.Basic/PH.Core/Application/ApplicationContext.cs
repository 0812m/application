using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PH.Core.Application;
using PH.Core.Application.Attributes;
using PH.Core.Application.StartupModule;
using PH.ToolsLibrary.Reflection;

namespace PH.Core
{
    public static class ApplicationContext
    {
        /// <summary>
        /// 入口程序集
        /// </summary>
        public static  Assembly EntryAssembly { get; set; }

        public static IEnumerable<Assembly> Assemblies { get; set; }

        /// <summary>
        /// 控制器
        /// </summary>
        public static ConcurrentDictionary<string, Type> Controllers { get; set; }

        /// <summary>
        /// 主机环境
        /// </summary>
        public static IWebHostEnvironment WebHostingEnvironment { get; set; }

        /// <summary>
        /// 主机环境
        /// </summary>
        public static IHostEnvironment HostingEnvironment { get; set; }

        /// <summary>
        /// 全局配置构建器
        /// </summary>
        public static IConfigurationBuilder ConfigurationBuilder { get; set; }

        /// <summary>
        /// 全局应用设置选项
        /// </summary>
        public static ApplicationSetting ApplicationSetting { get; set; }

        /// <summary>
        /// 全局配置
        /// </summary>
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// 全局服务提供器
        /// </summary>
        public static IServiceProvider ApplicationServices { get; set; }

        /// <summary>
        /// 过滤器
        /// </summary>
        public static IEnumerable<Type> Filters { get; set; }

        /// <summary>
        /// 排除运行时 Json 后缀
        /// </summary>
        internal static readonly string[] runtimeJsonSuffixs = new[]
        {
            "deps.json",
            "runtimeconfig.dev.json",
            "runtimeconfig.prod.json",
            "runtimeconfig.json"
        };

        /// <summary>
        /// ASP.NET 5 内置环境标识
        /// </summary>
        internal static readonly string[] internalEnvironments = new[]
        {
            "Development",
            "Staging",
            "Production"
        };

        /// <summary>
        /// 应用所有启动配置对象
        /// </summary>
        internal static List<IStartupModule> AppStartups;

        static ApplicationContext()
        {
            Controllers = new ConcurrentDictionary<string, Type>();

            EntryAssembly = ReflectionUtil.GetEntryAssmbly();

             Assemblies = ReflectionUtil.GetDependencyAssemblies(x => (!x.Serviceable && x.Type == "project") || x.Name.StartsWith("PH."))
                .Where(x => !x.IsDefined(typeof(SkipScanAttribute)));

            FindAppStartups();

            FindFilters();
        }

        private static void FindAppStartups() 
        {
            AppStartups = Assemblies.SelectMany(x => x.GetTypes())
                .Where(x => typeof(IStartupModule).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract && !x.IsDefined(typeof(SkipScanAttribute)))
                .Select(x => Activator.CreateInstance(x) as IStartupModule).OrderBy((x) => x.GetAttribute<StartupAttribute>()?.Order??int.MaxValue).ToList();
        }

        private static void FindFilters() 
        {
           Filters = Assemblies.SelectMany(x => x.GetTypes())
                .Where(x => x.IsClass && x.IsPublic && !x.IsAbstract && !x.IsDefined(typeof(SkipScanAttribute)) && typeof(IFilterMetadata).IsAssignableFrom(x)).ToList();
        }
    }
}
