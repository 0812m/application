using PH.Web.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.Dtos
{
    public class ToolsDto:Dto
    {
        /// <summary>
        /// 图片
        /// </summary>
        public virtual string Image { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public virtual string Url { get; set; }

        /// <summary>
        /// 工具名
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 能否下载
        /// </summary>
        public virtual bool CanDownload { get; set; }
    }
}
