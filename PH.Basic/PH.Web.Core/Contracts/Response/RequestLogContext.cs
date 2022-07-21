using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PH.Core.IOC.Attributes;
using PH.Core.IOC.Enum;

namespace PH.Web.Core.Contracts
{
    [AutoInjection(patterns: InjectionPatterns.Self)]
    public class RequestLogContext
    {
        /// <summary>
        /// 应用程序名
        /// </summary>
        public  string Application
        {
            get
            {
                return PH.Core.ApplicationContext.WebHostingEnvironment.ApplicationName;
            }
        }

        /// <summary>
        /// 异常
        /// </summary>
        public List<Exception>  Exceptions { get; set; }

        /// <summary>
        /// 响应内容
        /// </summary>
        public Response  Response { get; set; }

        /// <summary>
        /// ActionResult是否支持 规范统一结果
        /// </summary>
        public bool CanSpecification { get; set; }
    }
}
