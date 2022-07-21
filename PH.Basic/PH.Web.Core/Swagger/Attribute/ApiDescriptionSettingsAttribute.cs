using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PH.Web.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiDescriptionSettingsAttribute: ApiExplorerSettingsAttribute
    {
        /// <summary>
        /// 模块内分组
        /// </summary>
        public virtual string Grouping { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public virtual string Version { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public virtual string Contact { get; set; }

        /// <summary>
        /// 联系人邮箱
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// 标题（仅限于控制器）
        /// </summary>
        public virtual string Title { get; set; }

        public virtual string ModuleName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleName">模块</param>
        public ApiDescriptionSettingsAttribute(string moduleName)
        {
            GroupName = moduleName;
            Contact = "www.phqmo.com";
            Email = "321304825@qq.com";
        }
    }
}
