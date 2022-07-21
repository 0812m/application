using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PH.Web.Core.Mvc.DynamicWebApi
{
    internal class DynamicApiControllerFeatureProvider: ControllerFeatureProvider
    {
        protected override bool IsController(TypeInfo typeInfo)
        {
            return typeof(IDynamicApi).IsAssignableFrom(typeInfo)
                 && typeInfo.IsClass// 控制器必须是类 
                 && typeInfo.IsPublic //且是公开的
                 && !typeInfo.IsAbstract //为保证可实例化，排除掉抽象类
                 && !typeInfo.IsGenericType//泛型类也不允许，因为需要确切的类型才能实例化
                 && !typeInfo.IsDefined(typeof(NonControllerAttribute),false);
        }
    }
}
