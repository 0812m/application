using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.Options;
using PH.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace PH.Web.Core.Mvc.DynamicWebApi
{
    internal class DynamicApiApplicationServiceConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                if (typeof(IDynamicApi).IsAssignableFrom(controller.ControllerType))
                    ConfigureDynamicApi(controller);
            }
        }

        private void ConfigureDynamicApi(ControllerModel controller)
        {
            MvcCommon.ConfigureControllerName(controller);
            ConfigureApiExplorer(controller);
            ConfigureSelector(controller);
            ConfigureParameters(controller);
        }

        /// <summary>
        /// 配置选择器
        /// </summary>
        /// <param name="controller"></param>
        private void ConfigureSelector(ControllerModel controller)
        {
            RemoveEmptySelectors(controller.Selectors);

            foreach (var action in controller.Actions)
            {
                ConfigureSelector(action);
            }
        }
        
        /// <summary>
        /// 配置选择器
        /// </summary>
        /// <param name="action"></param>
        private void ConfigureSelector(ActionModel action)
        {
            if (action.Attributes.Any(x => x.GetType() == typeof(NonActionAttribute)))
                return;

            RemoveEmptySelectors(action.Selectors);

            if (action.Selectors.Count <= 0)
            {
                AddDynamicApiSelector(action);
            }
            else
            {
                NormalizeSelectorRoutes(action);
            }
        }

        /// <summary>
        /// 添加选择器
        /// </summary>
        /// <param name="action"></param>
        private void AddDynamicApiSelector(ActionModel action)
        {
            var selector = new SelectorModel();
            selector.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(CalculateRouteTemplate(action)));
            selector.ActionConstraints.Add(new HttpMethodActionConstraint(new[] { GetHttpMethod(action) }));

            action.Selectors.Add(selector);
        }

        /// <summary>
        /// 计算路由
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private string CalculateRouteTemplate(ActionModel action)
        {
            var routeTemplate = new StringBuilder();
            //controller部分
            if (action.Controller.Attributes.All(x => x as RouteAttribute == null))
                routeTemplate.Append($"api/{action.Controller.ControllerName.ToLower()}");
            //else
            //{
            //    var routeAttr = action.Controller.Attributes.FirstOrDefault(x => x.GetType() == typeof(RouteAttribute)) as RouteAttribute;
            //    var controllerRouteTemplate = routeAttr.Template.Replace("[controller]", action.Controller.ControllerName.ToLower());
            //    routeTemplate.Append(new string(controllerRouteTemplate.ToCharArray()));
            //}

            // Action 名称部分
            var actionName = action.ActionName.ToLower();
            if (actionName.EndsWith("async"))
            {
                actionName = actionName[..(actionName.Length - 4)];
            }
            var trimPrefixes = new[]
            {
                "GetAll","GetList","Get","Search",
                "Post","Create","Add","Insert",
                "Put","Update",
                "Delete","Remove",
                "Patch"
            };
            var prefix = string.Empty;
            if ((prefix = trimPrefixes.FirstOrDefault(x => actionName.StartsWith(x,StringComparison.OrdinalIgnoreCase))) != null)
                actionName = actionName[prefix.Length..];
            if (!string.IsNullOrEmpty(actionName))
                routeTemplate.Append($"/{actionName}");
            if (!string.IsNullOrWhiteSpace(prefix))
                routeTemplate.Append($"/{prefix.ToLower()}");

            // id 部分
            if (action.Parameters.Any(temp => temp.ParameterName == "id"))
            {
                routeTemplate.Append("/{id}");
            }

            return routeTemplate.ToString();
        }

        /// <summary>
        /// 选择 HttpMethod
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private string GetHttpMethod(ActionModel action)
        {
            var actionName = action.ActionName;
            if (actionName.StartsWith("Get", StringComparison.OrdinalIgnoreCase) || actionName.StartsWith("Search", StringComparison.OrdinalIgnoreCase))
            {
                return "GET";
            }

            if (actionName.StartsWith("Put", StringComparison.OrdinalIgnoreCase) || actionName.StartsWith("Update", StringComparison.OrdinalIgnoreCase))
            {
                return "PUT";
            }

            if (actionName.StartsWith("Delete", StringComparison.OrdinalIgnoreCase) || actionName.StartsWith("Remove", StringComparison.OrdinalIgnoreCase) || actionName.StartsWith("Del", StringComparison.OrdinalIgnoreCase) || actionName.StartsWith("Rm", StringComparison.OrdinalIgnoreCase))
            {
                return "DELETE";
            }

            if (actionName.StartsWith("Patch", StringComparison.OrdinalIgnoreCase))
            {
                return "PATCH";
            }

            return "POST";
        }

        /// <summary>
        /// 配置参数
        /// </summary>
        /// <param name="controller"></param>
        private void ConfigureParameters(ControllerModel controller)
        {
            foreach (var action in controller.Actions)
            {
                foreach (var parameter in action.Parameters)
                {
                    if (parameter.BindingInfo != null)
                    {
                        continue;
                    }

                    if (parameter.ParameterType.IsClass &&
                        parameter.ParameterType != typeof(string) &&
                        parameter.ParameterType != typeof(IFormFile))
                    {
                        var httpMethods = action.Selectors.SelectMany(temp => temp.ActionConstraints).OfType<HttpMethodActionConstraint>().SelectMany(temp => temp.HttpMethods).ToList();
                        if (httpMethods.Contains("GET") ||
                            httpMethods.Contains("DELETE") ||
                            httpMethods.Contains("TRACE") ||
                            httpMethods.Contains("HEAD"))
                        {
                            continue;
                        }

                        parameter.BindingInfo = BindingInfo.GetBindingInfo(new[] { new FromBodyAttribute() });
                    }
                }
            }
        }

        /// <summary>
        /// Api 可见性设置
        /// </summary>
        public void ConfigureApiExplorer(ControllerModel controller)
        {
            if (!controller.ApiExplorer.IsVisible.HasValue)
            {
                controller.ApiExplorer.IsVisible = true;
            }

            foreach (var action in controller.Actions)
            {
                if (!action.ApiExplorer.IsVisible.HasValue)
                {
                    action.ApiExplorer.IsVisible = true;
                }
            }
        }

        /// <summary>
        /// 删除无效（空白）选择器
        /// </summary>
        /// <param name="selectors"></param>
        private void RemoveEmptySelectors(IList<SelectorModel> selectors)
        {
            for (var i = selectors.Count - 1; i >= 0; i--)
            {
                var selector = selectors[i];
                //如果没有 路由信息，action 约束，终结点元数据 则删除选择器
                if (selector.AttributeRouteModel == null &&
                    (selector.ActionConstraints == null || selector.ActionConstraints.Count <= 0) &&
                    (selector.EndpointMetadata == null || selector.EndpointMetadata.Count <= 0))
                {
                    selectors.Remove(selector);
                }
            }
        }

        /// <summary>
        /// 规范 Api 路由
        /// </summary>
        /// <param name="action"></param>
        private void NormalizeSelectorRoutes(ActionModel action)
        {
            foreach (var selector in action.Selectors)
            {
                if (selector.AttributeRouteModel == null)
                {
                    selector.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(CalculateRouteTemplate(action)));
                }

                if (selector.ActionConstraints.OfType<HttpMethodActionConstraint>().FirstOrDefault()?.HttpMethods?.FirstOrDefault() == null)
                {
                    selector.ActionConstraints.Add(new HttpMethodActionConstraint(new[] { GetHttpMethod(action) }));
                }
            }
        }
    }
}
