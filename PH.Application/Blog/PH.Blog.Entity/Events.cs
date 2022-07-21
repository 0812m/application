using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PH.DatabaseAccessor.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Entity
{
    /// <summary>
    /// 事件实体
    /// </summary>
    public class Events:CanDelEntity<Events,int>
    {
        /// <summary>
        /// 事件名
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 相关ID
        /// </summary>
        public virtual int RelatedId { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public virtual string Content { get; set; }

        /// <summary>
        /// 事件类型
        /// </summary>
        public virtual int Type { get; set; }

        protected override void Mapping(EntityTypeBuilder<Events> entityTypeBuilder)
        {
            entityTypeBuilder.Property(x => x.IsDeleted).HasColumnName("deleted");
        }
    }
}
