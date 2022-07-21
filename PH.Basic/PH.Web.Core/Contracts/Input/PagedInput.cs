using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Web.Core.Contracts
{
    public class PagedInput:Input
    {
        /// <summary>
        /// 页容量
        /// </summary>
        public virtual int PageSize { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public virtual int PageIndex { get; set; }
    }
}
