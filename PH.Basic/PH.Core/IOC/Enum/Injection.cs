using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace PH.Core.IOC.Enum
{
    /// <summary>
    /// 注入方式 
    /// </summary>
   public enum Injection
    {
        [Description("单例")]
        Singleton,
        [Description("范围")]
        Scoped,
        [Description("瞬时")]
        Transient
    }

    /// <summary>
    /// 注入方法
    /// </summary>
    public enum InjectionActions 
    {
        Add,
        TryAdd
    }

    public enum InjectionPatterns 
    {
        [Description("只注册自己")]
        Self,

        [Description("只注册第一个接口")]
        FirstInterface,

        [Description("自己和第一个接口")]
        SelfWithFirstInterface,

        [Description("所有接口")]
        ImplementedInterfaces,

        [Description("自己包括所有接口")]
        All
    }
}
