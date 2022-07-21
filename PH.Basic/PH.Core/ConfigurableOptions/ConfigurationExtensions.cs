using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using PH.Core.ConfigurableOptions;
using PH.Core;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddConfigurationOption<TOption>(this IServiceCollection services)
            where TOption : class, IConfigurableOptions
        {
            //1.解析'TOption'需绑定的Jsonkey
            var optionsType = typeof(TOption);
            var optionsSetting = optionsType.GetCustomAttribute<OptionsSettingAttribute>(false);
            var jsonKey = AnalysisJsonKey(optionsSetting, optionsType);
            var optionConfiguration = ApplicationContext.Configuration.GetSection(jsonKey);

            //2.配置选项监听
            if (optionsType.IsAssignableTo(typeof(IConfigurableOptionsListener<TOption>)))
            {
                var onListenerMethod = optionsType.GetMethod(nameof(IConfigurableOptionsListener<TOption>.OnListener));
                if (onListenerMethod != null)
                {
                    ChangeToken.OnChange(
                        () => ApplicationContext.Configuration.GetReloadToken(),
                        () =>
                    {
                        var option = optionConfiguration.Get<TOption>();
                        onListenerMethod.Invoke(option, new object[] { option, optionConfiguration });
                    });
                }
            }

            services.AddOptions<TOption>().Bind(optionConfiguration, builderOption =>
            {
                //绑定私有变量
                builderOption.BindNonPublicProperties = true;
            }).ValidateDataAnnotations();

            //3.配置后期复杂验证
            var validateInterface = optionsType.GetInterfaces().FirstOrDefault(x => x.IsGenericType && typeof(IConfigurableOptions).IsAssignableFrom(x.GetGenericTypeDefinition()));
            if (validateInterface != null)
            {
                var genericArguments = validateInterface.GenericTypeArguments;

                //配置后期配置
                if (genericArguments.Length > 1)
                    services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IValidateOptions<TOption>), genericArguments.Last()));

                var postConfigure = optionsType.GetMethod(nameof(IConfigurableOptions<TOption>.PostConfigure));
                if (postConfigure != null)
                    if (optionsSetting != null && optionsSetting.PostConfigureAll)
                        services.PostConfigureAll<TOption>(options => postConfigure.Invoke(options, new object[] { options, optionConfiguration }));
                    else
                        services.PostConfigure<TOption>(options => postConfigure.Invoke(options, new object[] { options, optionConfiguration }));
            }

            return services;
        }

        /// <summary>
        /// 解析jsonKey
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private static string AnalysisJsonKey(OptionsSettingAttribute optionsSetting, Type options)
        {
            //OptionsSettingAttribute 优先级最高
            if (optionsSetting != null && !string.IsNullOrWhiteSpace(optionsSetting.JsonKey))
                return optionsSetting.JsonKey;

            //其次是类名称
            var className = options.Name;
            if (className.EndsWith("option", StringComparison.OrdinalIgnoreCase))
                className = className.Replace("option", "", StringComparison.OrdinalIgnoreCase);
            if (className.EndsWith("options", StringComparison.OrdinalIgnoreCase))
                className = className.Replace("options", "", StringComparison.OrdinalIgnoreCase);

            return className;
        }
    }
}
