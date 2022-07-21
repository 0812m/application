using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Web.Core.Contracts
{
    public class PageOutput<T>
    {
        /// <summary>
        /// 页码
        /// </summary>
        public virtual int PageIndex { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        public virtual int PageSize { get; set; }

        /// <summary>
        /// 条目总数
        /// </summary>
        public virtual int Total { get; set; }

        /// <summary>
        /// 页总数
        /// </summary>
        public virtual int PageTotal
        { 
            get 
            {
                return (Total + PageSize - 1) / PageSize;
            } 
        }

        /// <summary>
        /// 数据
        /// </summary>
        public virtual IEnumerable<T> Items { get; set; }
    }
}
