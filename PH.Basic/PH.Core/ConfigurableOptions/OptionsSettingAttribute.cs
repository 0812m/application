using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Core.ConfigurableOptions
{
    /// <summary>
    /// 选项配置特性
    /// </summary>
   public class OptionsSettingAttribute:Attribute
    {
        public OptionsSettingAttribute()
        {

        }

        public OptionsSettingAttribute(string jsonKey)
        {
            JsonKey = jsonKey;
        }

        public OptionsSettingAttribute(bool postConfigureAll)
        {
            PostConfigureAll = postConfigureAll;
        }

        public OptionsSettingAttribute(string jsonKey, bool postConfigureAll)
        {
            JsonKey = jsonKey;
            PostConfigureAll = postConfigureAll;
        }

        public string JsonKey { get; set; }

        public bool PostConfigureAll { get; set; }
    }
}
