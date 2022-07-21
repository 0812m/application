using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Core.Application.Attributes;

namespace PH.Web.Core.Contracts
{
    /// <summary>
    /// 跳过规范结果
    /// </summary>
    [SkipScan]
    public class UnnecessarySpecificationResutlAttribute: Attribute
    {

    }
}
