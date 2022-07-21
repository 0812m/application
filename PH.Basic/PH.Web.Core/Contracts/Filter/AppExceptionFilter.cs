using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Web.Core.Contracts.Filter
{
    public class AppExceptionFilter : IAsyncExceptionFilter
    {
        public AppExceptionFilter()
        {
        }


        public async Task OnExceptionAsync(ExceptionContext context)
        {
        }
    }
}
