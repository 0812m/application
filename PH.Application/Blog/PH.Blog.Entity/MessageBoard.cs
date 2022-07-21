using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PH.DatabaseAccessor.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Entity
{
    /// <summary>
    /// 留言板
    /// </summary>
    public class MessageBoard:CanDelEntity<MessageBoard,int>
    {
        public virtual string Content { get; set; }

        public virtual string? Contact { get; set; }

        public virtual string? IP { get; set; }

        public virtual string? NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public virtual string Avatar { get; set; }

        protected override void Mapping(EntityTypeBuilder<MessageBoard> entityTypeBuilder)
        {
            entityTypeBuilder.Property(x => x.IsDeleted).HasColumnName("deleted");
            entityTypeBuilder.Property(x => x.Contact).HasDefaultValue("");
            entityTypeBuilder.Property(x => x.NickName).HasDefaultValue("");
        }
    }
}
