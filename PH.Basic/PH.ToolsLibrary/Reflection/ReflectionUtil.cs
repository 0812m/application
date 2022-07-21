using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PH.ToolsLibrary.Reflection
{
    public static class ReflectionUtil
    {
        /// <summary>
        /// 获取所有实现类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<Type> FindAllImplementClass<IT>(Predicate<Type> predicate = null)
            where IT : class
        {
            var type = typeof(IT);
            if (!type.IsInterface)
                throw new Exception("T 必须是接口");

            var types = GetDependencyAssemblies().SelectMany(x => x.GetTypes()).Where(x => type.IsAssignableFrom(x));
            if (predicate is not null)
                types = types.Where(x => predicate(x));
            return types;
        }

        /// <summary>
        /// 获取程序入口程序集
        /// </summary>
        /// <returns></returns>
        public static Assembly GetEntryAssmbly()
        {
            return Assembly.GetEntryAssembly();
        }

        /// <summary>
        /// 从程序入口开始获取所有依赖程序集
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetDependencyAssemblies(Func<RuntimeLibrary, bool> predicate = default)
        {
            var compileLibraries = DependencyContext.Default.RuntimeLibraries.AsEnumerable();
            if (predicate is not null)
                compileLibraries = compileLibraries.Where(predicate);

            return compileLibraries.Select(x =>
             {
                 Assembly assembly = default;
                 try
                 {
                     assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(x.Name));
                 }
                 catch (Exception)
                 {

                 }
                 return assembly;
             }).Where(x => x is not null);
        }

        /// <summary>
        /// 提取属性名称及值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IDictionary<string, object> GetPropertieValues(this object obj)
        {
            var t = obj.GetType();
            var props = t.GetProperties();

            IDictionary<string, object> dics = new Dictionary<string, object>();

            if (props is not null && props.Any())
                foreach (var prop in props)
                {
                    dics.Add(prop.Name, prop.GetValue(obj));
                }

            return dics;
        }

        /// <summary>
        /// IEnumerable<Claim> 创建 T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static T AS<T>(this IEnumerable<Claim> claims)
            where T : class,new()
        {
            var t = typeof(T);
            var props = t.GetProperties();
            var Instance = Activator.CreateInstance<T>();

            foreach (var prop in props)
            {
                var name = prop.Name;
                var propType = prop.PropertyType;
                var claimValue = claims.FirstOrDefault(x => x.Type.ToLower() == name.ToLower())?.Value;
                if (claimValue is null)
                    continue;
                var value = Convert.ChangeType(claimValue, propType);
                prop.SetValue(Instance, value);
            }
            return Instance;
        }
    }
}
