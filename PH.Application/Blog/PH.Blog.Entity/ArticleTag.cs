using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PH.DatabaseAccessor.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Entity
{
    public class ArticleTag : Entity<ArticleTag>
    {
        /// <summary>
        /// 标签id
        /// </summary>
        public virtual int TagId { get; set; }

        /// <summary>
        /// 文章id
        /// </summary>
        public virtual int ArticleId { get; set; }

        protected override void Mapping(EntityTypeBuilder<ArticleTag> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => new { x.ArticleId, x.TagId });

            entityTypeBuilder.HasOne<Article>().WithMany(x => x.Tags).HasForeignKey(x => x.ArticleId);
            entityTypeBuilder.HasOne<Tag>().WithMany(x => x.Articles).HasForeignKey(x => x.TagId);
        }
    }
}
