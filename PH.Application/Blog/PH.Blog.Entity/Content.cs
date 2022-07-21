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
    /// 内容
    /// </summary>
    public class Content : Entity<Content, int>
    {
        /// <summary>
        /// 文章id
        /// </summary>
        public virtual int ArticleId { get; set; }

        /// <summary>
        /// text
        /// </summary>
        public virtual string Text { get; set; }

        /// <summary>
        /// html
        /// </summary>
        public virtual string Markdown { get; set; }

        /// <summary>
        /// 文章实体
        /// </summary>
        public virtual Article  Article { get; set; }

        protected override void Mapping(EntityTypeBuilder<Content> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);

            entityTypeBuilder.HasOne(x => x.Article).WithOne(x => x.Content).HasForeignKey<Content>(x => x.ArticleId);
            entityTypeBuilder.Property(x => x.Text).IsRequired();
            entityTypeBuilder.Property(x => x.Markdown).IsRequired();
        }
    }
}
