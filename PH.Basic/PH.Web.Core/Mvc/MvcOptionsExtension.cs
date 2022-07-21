using Microsoft.AspNetCore.Mvc;
using PH.Web.Core.Contracts.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Core;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace PH.Web.Core.Mvc
{
    public class MvcOptionsExtension
    {
        public static Action<MvcOptions> Configure()
        {
            return options => 
            {
                //注册过滤器
                foreach (var filter in ApplicationContext.Filters)
                {
                    options.Filters.Add(filter);
                }
            };
        }
    }
}
