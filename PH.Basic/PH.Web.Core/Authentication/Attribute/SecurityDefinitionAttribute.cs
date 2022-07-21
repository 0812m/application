using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Web.Core.Authentication
{
    /// <summary>
    /// 安全定义特性
    /// </summary>
    public class SecurityDefinitionAttribute: Attribute
    {
        public readonly string Permission;

        public SecurityDefinitionAttribute(string permission)
        {
            Permission = permission;
        }
    }
}
