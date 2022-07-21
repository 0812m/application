using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Core.ConfigurableOptions;

namespace PH.Core.Application
{
    [OptionsSetting("AppSetting")]
  public class ApplicationSetting : IConfigurableOptions<ApplicationSetting>
    {
        /// <summary>
        /// 是否使用动态 API
        /// </summary>
        public  bool UseDynamicWebApi { get; set; }

        /// <summary>
        /// 是否使用 Swagger 文档
        /// </summary>
        public bool UseSwaggerDoc { get; set; }

        public void PostConfigure(ApplicationSetting options, IConfiguration configuration)
        {
            UseDynamicWebApi = true;
            UseSwaggerDoc = true;
        }
    }
}
