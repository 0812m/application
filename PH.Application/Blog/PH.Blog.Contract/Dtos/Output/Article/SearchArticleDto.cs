using PH.Web.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.Dtos
{
    public class SearchArticleDto:Dto
    {
        public virtual int Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public virtual string Cover { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public virtual string Opening { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public virtual string ReleaseTime { get; set; }

        /// <summary>
        /// 阅读量
        /// </summary>
        public virtual int Pageviews { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public virtual IEnumerable<string> Tags { get; set; }
    }
}
