using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PH.DatabaseAccessor;
using PH.DatabaseAccessor.Repository;
using PH.DatabaseAccessor.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DatabaseAccessorExtension
    {
        /// <summary>
        /// 注册 EFCore 仓储
        /// </summary>
        /// <param name="services"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IServiceCollection AddDatabaseAccessor(this IServiceCollection services, Action<IServiceCollection> action)
        {
            action?.Invoke(services);

            services.AddScoped<IDbContextPool, DbContextPool>();
            services.AddScoped(typeof(IReadOnlyRepository<>), typeof(ReadOnlyEFCoreRepository<>));
            services.AddScoped(typeof(IRepository<>),typeof(EFCoreRepository<>));
            services.AddScoped<IEFCoreRepositoryFactory, EFCoreRepositoryFactory>();
            services.AddScoped<IRepositoryFactory,EFCoreRepositoryFactory>();

            //根据定位器获取
            services.AddTransient(IServiceProvider =>
            {
                Func<Type, DbContext> func = locator =>
                 {
                     var isRegister = Penetrates.DbContextWithLocatorCached.TryGetValue(locator, out var dbcontextType);
                     if (!isRegister)
                         throw new Exception($"The DbContext for locator `{locator.FullName}` unregistered");

                     var appDbContextAttribute = dbcontextType.GetCustomAttribute<AppDbContextAttribute>();
                     var dbContextPool = IServiceProvider.GetService<IDbContextPool>();
                     var dbContext = IServiceProvider.GetService(dbcontextType) as DbContext;

                     if (appDbContextAttribute.IsDynamic)
                         DynamicModelCacheKeyFactory.RefreshModel();
                     if (dbContext is not null)
                         dbContextPool?.AddToPool(dbContext);
                     return dbContext;
                 };
                return func;
            });

            //获取指定DbContext
            services.AddTransient(IServiceProvider =>
            {
                Func<Type, bool,DbContext> func = (dbContextType, notLocator) =>
                {
                    var targetDbContext = Penetrates.DbContextWithLocatorCached.Values.FirstOrDefault(t => t == dbContextType);
                    if (targetDbContext is null)
                        targetDbContext = Penetrates.DbContextWithAttributeCached.Values.FirstOrDefault(t => t == dbContextType);
                    if (targetDbContext is null)
                        throw new Exception($"The DbContext `{dbContextType.Name}` unregistered");

                    var appDbContextAttribute = targetDbContext.GetCustomAttribute<AppDbContextAttribute>();
                    var dbContextPool = IServiceProvider.GetService<IDbContextPool>();
                    var dbContext = IServiceProvider.GetService(targetDbContext) as DbContext;

                    if (appDbContextAttribute.IsDynamic)
                        DynamicModelCacheKeyFactory.RefreshModel();
                    if (dbContext is not null)
                        dbContextPool?.AddToPool(dbContext);
                    return dbContext;
                };
                return func;
            });

            //根据 AppDbContextAttribute 获取
            services.AddTransient(IServiceProvider =>
            {
                Func<Func<AppDbContextAttribute,bool>, DbContext> func = predicate =>
                {
                    var key = Penetrates.DbContextWithAttributeCached.Keys.FirstOrDefault(predicate);
                    Penetrates.DbContextWithAttributeCached.TryGetValue(key,out var targetDbContext);
                       
                    if (targetDbContext is null)
                        throw new Exception($"The DbContext  unregistered");

                    var appDbContextAttribute = targetDbContext.GetCustomAttribute<AppDbContextAttribute>();
                    var dbContextPool = IServiceProvider.GetService<IDbContextPool>();
                    var dbContext = IServiceProvider.GetService(targetDbContext) as DbContext;

                    if (appDbContextAttribute.IsDynamic)
                        DynamicModelCacheKeyFactory.RefreshModel();
                    if (dbContext is not null)
                        dbContextPool?.AddToPool(dbContext);
                    return dbContext;
                };
                return func;
            });

            return services;
        }

        /// <summary>
        /// 注册 DbContext 上下文
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="poolSize">DbContextPool 最大数量</param>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="interceptors">拦截器</param>
        /// <returns></returns>
        public static IServiceCollection AddDbContextToPool<TDbContext>
            (
            this IServiceCollection services
            , int poolSize = 128,
            string connectionString = null,
            IEnumerable<IInterceptor> interceptors = null,
            ILoggerFactory loggerFactory = null
            )
            where TDbContext : DbContext
        {
            var dbContext = typeof(TDbContext);
            var appDbContextAttribute = dbContext.GetCustomAttribute<AppDbContextAttribute>();
            if (appDbContextAttribute is null) throw new CustomAttributeFormatException("Not found  `AppDbContextAttribute`");

            services.RegisterDbContext<TDbContext>()
                .AddDbContextPool<TDbContext>((serviceProvider, builder) => 
                {
                    builder.ConfiguringDbContext<TDbContext>(serviceProvider, appDbContextAttribute, connectionString, interceptors, loggerFactory);

                }, poolSize);
            
            return services;
        }

        internal static IServiceCollection RegisterDbContext<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
           var type = typeof(TDbContext);
           var baseType = type;

            while (baseType.BaseType !=  typeof(DbContext)) 
                baseType = baseType.BaseType;
            if (baseType.GenericTypeArguments.Length == 2)
            {
               var locator = baseType.GenericTypeArguments[1];
               var isSuccess = Penetrates.DbContextWithLocatorCached.TryAdd(locator, type);
                if (!isSuccess) throw new InvalidOperationException($"`{locator.Name}`定位器与与其他DbContext绑定");
            }

            if (type.IsDefined(typeof(AppDbContextAttribute)))
                Penetrates.DbContextWithAttributeCached.TryAdd(type.GetCustomAttribute<AppDbContextAttribute>(), type);
            else
                throw new CustomAttributeFormatException("Not found  `AppDbContextAttribute`");

            return services;
        }
    }
}
