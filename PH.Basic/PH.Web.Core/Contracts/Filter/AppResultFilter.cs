using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Web.Core.Contracts.Filter
{
    public class AppResultFilter : IAsyncResultFilter
    {
        public AppResultFilter()
        {
        }


        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
           var controllerAction = context.ActionDescriptor as ControllerActionDescriptor;
           var unnecessarySpecificationResutlAttribute = controllerAction.EndpointMetadata.Any(x => x.GetType() == typeof(UnnecessarySpecificationResutlAttribute));

            if (!unnecessarySpecificationResutlAttribute)
            {
                var canNormalize = context.Result is ObjectResult || context.Result is ContentResult || context.Result is EmptyResult;
                if (canNormalize)
                {
                    ObjectResult result = context.Result is EmptyResult ? null : context.Result as ObjectResult;
                    Response response = new Response(result?.Value);
                    context.Result = new ObjectResult(response);
                }
            }

            await next();
        }
    }
}
