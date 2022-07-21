using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Core.ConfigurableOptions
{
    public partial interface IConfigurableOptions { }

    /// <summary>
    /// 配置绑定依赖接口
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public partial interface IConfigurableOptions<TOptions>: IConfigurableOptions
        where TOptions : class, IConfigurableOptions
    {
        void PostConfigure(TOptions options, IConfiguration configuration);
    }

    /// <summary>
    /// 后期配置验证依赖接口
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <typeparam name="TOptionsValidation"></typeparam>
    public partial interface IConfigurableOptions<TOptions, TOptionsValidation> : IConfigurableOptions<TOptions>
        where TOptions : class, IConfigurableOptions
    { }

    /// <summary>
    /// 配置监听依赖接口
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public partial interface IConfigurableOptionsListener<TOptions> : IConfigurableOptions<TOptions>
        where TOptions : class, IConfigurableOptions
    {
        void OnListener(TOptions options, IConfiguration configuration);
    }
}
