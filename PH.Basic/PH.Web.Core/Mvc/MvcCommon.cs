using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Web.Core.Mvc
{
    internal class MvcCommon
    {
        /// <summary>
        /// 配置控制器名称
        /// </summary>
        /// <param name="controller"></param>
        internal static void ConfigureControllerName(ControllerModel controller)
        {
            controller.ControllerName = TrimControllerNameEnd(controller.ControllerName);
        }

        internal static string TrimControllerNameEnd(string name)
        {
            if(string.IsNullOrWhiteSpace(name)) return name;

            if (name.ToLower().EndsWith("apis"))
            {
                name = name[..(name.Length - 4)];
            }
            else if (name.ToLower().EndsWith("api"))
            {
                name = name[..(name.Length - 3)];
            }
            return name.ToLower();
        }
    }
}
