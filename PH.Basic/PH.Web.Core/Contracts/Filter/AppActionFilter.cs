using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Web.Core.Contracts.Filter
{
    public class AppActionFilter : IAsyncActionFilter, IOrderedFilter
    {
        public int Order => 1;

        public AppActionFilter()
        {

        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var actionArguments =  context.ActionArguments;
            if (actionArguments is null || actionArguments.Count <= 0)
                goto next;

            foreach (var argKeyValue in actionArguments)
            {
                var type = argKeyValue.Value.GetType();
                if (type.IsPrimitive || type == typeof(string))
                    continue;

                if (type.IsClass && typeof(IInput).IsAssignableFrom(type))
                {
                    var VerificationMethod = type.GetMethods()?.FirstOrDefault(x => x.Name == nameof(Input.Verification) && x.ReturnType == typeof(void) && (x.GetParameters() is null || x.GetParameters().Length <= 0));
                    VerificationMethod.Invoke(argKeyValue.Value, null);
                }
            }

           next: await next();
        }
    }
}
