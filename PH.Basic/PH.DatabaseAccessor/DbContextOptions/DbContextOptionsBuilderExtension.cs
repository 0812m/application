using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PH.DatabaseAccessor
{
    /// <summary>
    /// DbContextOptionsBuilder 配置扩展类
    /// </summary>
    internal static class DbContextOptionsBuilderExtension
    {

        /// <summary>
        /// 通过 AppDbContextAttribute 特性获取链接字符串
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static string GetConnectionString<TDbContext>(IServiceProvider serviceProvider, AppDbContextAttribute dbContextAttribute, string connectionString = null)
            where TDbContext : DbContext
        {
            if (!string.IsNullOrWhiteSpace(connectionString)) return connectionString;

            connectionString = dbContextAttribute?.ConnectionString;

            //如果包含 “=”号则认为是连接字符串
            if (connectionString.Contains("=")) return connectionString;
            else
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                if (connectionString.Contains(":")) return configuration[connectionString];
                else
                {
                    var connStr = configuration.GetConnectionString(connectionString);
                    return !string.IsNullOrWhiteSpace(connStr) ? connStr : configuration[connectionString];
                }
            }
        }

        /// <summary>
        /// 配置 DbContext
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="optionsBuilder"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="connectionString"></param>
        /// <param name="interceptors"></param>
        /// <returns></returns>
        public static DbContextOptionsBuilder ConfiguringDbContext<TDbContext>
            (
            this DbContextOptionsBuilder optionsBuilder,
            IServiceProvider serviceProvider,
            AppDbContextAttribute appDbContextAttribute,
            string connectionString,
            IEnumerable<IInterceptor> interceptors,
            ILoggerFactory loggerFactory = null
            )
            where TDbContext : DbContext
        {
            //1.日志
            optionsBuilder.EnableDetailedErrors();
            if (loggerFactory is not null)  optionsBuilder.UseLoggerFactory(loggerFactory);

            //2.注入拦截器，理论上应根据 IDbContextLocator 注入不同拦截器实现 DbContext 对数据库的读写权限控制
            optionsBuilder.AddInterceptors(interceptors ?? GetDefaultInterceptors());

            //4.调用 Usexxx 方法
            optionsBuilder.UseDatabaseProvider<TDbContext>(serviceProvider, appDbContextAttribute, connectionString);

            if (appDbContextAttribute.IsDynamic)
                optionsBuilder.ReplaceService<IModelCacheKeyFactory,DynamicModelCacheKeyFactory>();

            return optionsBuilder;
        }

        /// <summary>
        /// 调用 Usexxx 方法
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        public static DbContextOptionsBuilder UseDatabaseProvider<TDbContext>(this DbContextOptionsBuilder optionsBuilder, IServiceProvider serviceProvider, AppDbContextAttribute dbContextAttribute,string connectionString)
            where TDbContext : DbContext
        {
          var dll =  Directory.GetFiles(Directory.GetCurrentDirectory(),"*.dll",SearchOption.AllDirectories)
                ?.FirstOrDefault(x => Path.GetFileNameWithoutExtension(x) == dbContextAttribute.DbProvide); 
            var databaseProvideAssembly = Assembly.LoadFrom(dll);

            //1.数据库提供服务扩展类名
            var databaseProviderServiceExtensionTypeName = dbContextAttribute?.DbProvide switch
            {
                DbProvide.MySQL => "MySQLDbContextOptionsExtensions",
                DbProvide.SqlServer => "SqlServerDbContextOptionsExtensions",
                _ => null
            };

            var databaseProviderServiceExtensionType = databaseProvideAssembly.GetType($"Microsoft.EntityFrameworkCore.{databaseProviderServiceExtensionTypeName}");

            //2. useXXX方法名
            var useMethodName = dbContextAttribute?.DbProvide switch
            {
                DbProvide.MySQL => $"Use{nameof(DbProvide.MySQL)}",
                DbProvide.SqlServer => $"Use{nameof(DbProvide.SqlServer)}",
                _ => null
            };

            MethodInfo useMethod = databaseProviderServiceExtensionType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(x => x.Name == useMethodName && !x.IsGenericMethod && x.GetParameters().Length > 1 && x.GetParameters()[1].ParameterType == typeof(string));

            //3.通过 AppDbContextAttribute 特性获取链接字符串
            connectionString = GetConnectionString<TDbContext>(serviceProvider,dbContextAttribute, connectionString);
           
            //4.执行 UseXXX 方法
            useMethod.Invoke(null, new object[] { optionsBuilder, connectionString,null });

            return optionsBuilder;
        }

        /// <summary>
        /// 获取默认拦截器 
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<IInterceptor> GetDefaultInterceptors()
        {
            return new List<IInterceptor>() { };
        }
    }
}
