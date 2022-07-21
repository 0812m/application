using Microsoft.Extensions.DependencyInjection.Extensions;
using PH.Core.Application.StartupModule;
using PH.Core.IOC.Enum;
using PH.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PH.Core.IOC.Attributes;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AppServiceCollectionExtensions
    {
        /// <summary>
        /// 初始化应用程序
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection Application(this IServiceCollection services) 
        {
            //添加自动注入
            services.AddAutoInjection();

            // 执行所有 Startup.ConfiguraService 
            services.UseStartups();

            return services;
        }

        /// <summary>
        /// 自动注入
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        internal static IServiceCollection AddAutoInjection(this IServiceCollection services)
        {
            //获取所有贴有注入特性的‘Class’
            var types = ApplicationContext.Assemblies.SelectMany(t => t.GetTypes()).Where(x => x.IsClass
                                                                                                            && !x.IsAbstract
                                                                                                            && x.GetInterfaces().Count() > 0
                                                                                                            && x.IsDefined(typeof(AutoInjectionAttribute), false));

            foreach (var type in types)
            {
                //获取需要被排除的接口
                var autoInjection = type.GetCustomAttribute<AutoInjectionAttribute>();
                //获取‘Class’实现的接口，要么‘Class’与‘Interface’都是泛型且泛型参数数量一致，要么都不是泛型。
                var Interfaces = type.GetInterfaces().Where(x => ((x.IsGenericType && type.IsGenericType) 
                                                                                            && (x.GetGenericArguments().Length == type.GetGenericArguments().Length))
                                                                                            || (!x.IsGenericType && !type.IsGenericType) 
                                                                                            && !autoInjection.IgnoreType.Contains(x));

                //根据注入类型注入
                RegisterService(services, type, autoInjection, Interfaces);
            }

            return services;
        }

        internal static void RegisterService(IServiceCollection services, Type type, AutoInjectionAttribute autoInjection, IEnumerable<Type> interfaces = null)
        {
            List<Type> list = new List<Type>();

            switch (autoInjection.Patterns)
            {
                case InjectionPatterns.Self:
                    list.Add(type);
                    break;
                case InjectionPatterns.FirstInterface:
                   var t = interfaces.FirstOrDefault();
                    if(t is not null)
                        list.Add(t);
                    break;
                case InjectionPatterns.SelfWithFirstInterface:
                     t = interfaces.FirstOrDefault();
                    if (t is not null)
                        list.Add(t);
                    list.Add(type);
                    break;
                case InjectionPatterns.ImplementedInterfaces:
                    list.AddRange(interfaces);
                    break;
                case InjectionPatterns.All:
                    list.Add(type);
                    list.AddRange(interfaces);
                    break;
                default:
                    break;
            }

            RegisterType(services, type, autoInjection, list);
        }

        internal static void RegisterType(IServiceCollection services, Type type, AutoInjectionAttribute autoInjection, IEnumerable<Type> interfaces = null)
        {
            switch (autoInjection.Type)
            {
                case Injection.Singleton:
                    Singleton(services,type, autoInjection, interfaces);
                    break;
                case Injection.Scoped:
                    Scoped(services, type, autoInjection, interfaces);
                    break;
                case Injection.Transient:
                    Transient(services, type, autoInjection, interfaces);
                    break;
                default:
                    break;
            }
        }

        internal static void Singleton(IServiceCollection services, Type type, AutoInjectionAttribute autoInjection, IEnumerable<Type> interfaces = null) 
        {
            switch (autoInjection.Action)
            {
                case InjectionActions.Add:
                    Execute(type,interfaces,services.AddSingleton,null);
                    break;
                case InjectionActions.TryAdd:
                    Execute(type, interfaces,  null, services.TryAddSingleton);
                    break;
                default:
                    break;
            }
        }

        internal static void Execute(Type type, IEnumerable<Type> interfaces = null, Func<Type, Type
            , IServiceCollection> add = null, Action<Type, Type> tryAdd = null) 
        {
            foreach (var t in interfaces)
            {
                add?.Invoke(t,type);
                tryAdd?.Invoke(t,type);
            }
        }

        internal static void Scoped(IServiceCollection services, Type type, AutoInjectionAttribute autoInjection, IEnumerable<Type> interfaces = null)
        {
            switch (autoInjection.Action)
            {
                case InjectionActions.Add:
                    Execute(type, interfaces, services.AddScoped, null);
                    break;
                case InjectionActions.TryAdd:
                    Execute(type, interfaces, null, services.TryAddScoped);
                    break;
                default:
                    break;
            }
        }

        internal static void Transient(IServiceCollection services, Type type, AutoInjectionAttribute autoInjection, IEnumerable<Type> interfaces = null)
        {
            switch (autoInjection.Action)
            {
                case InjectionActions.Add:
                    Execute(type, interfaces, services.AddTransient, null);
                    break;
                case InjectionActions.TryAdd:
                    Execute(type, interfaces, null, services.TryAddTransient);
                    break;
                default:
                    break;
            }
        }

        internal static IServiceCollection UseStartups(this IServiceCollection services)
        {
            foreach (var startup in ApplicationContext.AppStartups)
            {
                var configureServicesMethod = startup.GetType()?.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .Where(m => m.ReturnType == typeof(void)
                                && m.GetParameters().Length > 0
                                && m.GetParameters().FirstOrDefault().ParameterType == typeof(IServiceCollection)
                                && m.Name == nameof(IStartupModule.ConfigureServices)).FirstOrDefault();

                if (configureServicesMethod is null) continue;

                configureServicesMethod.Invoke(startup, new object[] { services });
            }

            return services;
        }
    }
}
