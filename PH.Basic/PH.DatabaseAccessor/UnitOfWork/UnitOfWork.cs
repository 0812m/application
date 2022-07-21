using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using PH.ToolsLibrary.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PH.DatabaseAccessor.UnitOfWork
{
    public class UnitOfWorkFilter : IAsyncActionFilter, IOrderedFilter
    {
        public UnitOfWorkFilter(IDbContextPool dbContextPool)
        {
            DbContextPool = dbContextPool;
        }
        public int Order => 100;

        public IDbContextPool DbContextPool { get; }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
           var controllerAttr = context.Controller.GetAttribute<EnableTransactionAttribute>()?.EnableTransaction??false;
            var actionDesc = context.ActionDescriptor as ControllerActionDescriptor;
            var enableTransactionAttr = actionDesc.MethodInfo.GetCustomAttribute<EnableTransactionAttribute>();

           var actionExecutedContext = await next();

            //自动提交 DbContext
            DbContextPool.BeginTransaction(controllerAttr || (enableTransactionAttr?.EnableTransaction ?? false));
            DbContextPool.CommitTransaction(exception: actionExecutedContext?.Exception);
        }
    }
}
