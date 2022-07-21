using PH.Core.IOC.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Core.IOC.Attributes
{
    /// <summary>
    /// 自动注入特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AutoInjectionAttribute : Attribute
    {
        public AutoInjectionAttribute
            (
            Injection type = Injection.Scoped, 
            InjectionActions action = InjectionActions.Add, 
            InjectionPatterns patterns = InjectionPatterns.FirstInterface,
            params Type[] ignoreTypes
            )
        {
            Type = type;
            Action = action;
            Patterns = patterns;
            IgnoreType = ignoreTypes;
        }

        /// <summary>
        /// 注入方式
        /// </summary>
        public Injection Type;

        /// <summary>
        /// 注册方法
        /// </summary>
        public InjectionActions Action;

        /// <summary>
        /// 注册内容
        /// </summary>
        public InjectionPatterns Patterns;

        /// <summary>
        /// 忽略接口
        /// </summary>
        public Type[] IgnoreType;
    }
}
