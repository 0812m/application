using PH.Core;
using PH.ToolsLibrary.Json.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    internal  static class IMvcBuilderExtension
    {
        public static IMvcBuilder ConfigureIMvcBuilder(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddNewtonsoftJson(options => 
            {
                options.SerializerSettings.Converters.Add(new DateTimeNewtonsoftJsonConverter());
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            if (ApplicationContext.ApplicationSetting.UseDynamicWebApi)
                mvcBuilder.AddDynamicWebApi();

            return mvcBuilder;
        }
    }
}
