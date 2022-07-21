using PH.Web.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.Dtos
{
    public class EventDto:Dto
    {
        /// <summary>
        /// 事件名
        /// </summary>
        public virtual string Event { get; set; }

        /// <summary>
        /// 事件内容
        /// </summary>
        public virtual string Content { get; set; }

        /// <summary>
        /// 发生日期
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
    }
}
