using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PH.DatabaseAccessor.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PH.Blog.Entity
{
    /// <summary>
    /// 标签
    /// </summary>
    public class Tag : Entity<Tag, int>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 文章标签中间表
        /// </summary>
        public virtual IEnumerable<ArticleTag>  Articles { get; set; }

      
        protected override void Mapping(EntityTypeBuilder<Tag> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);

            entityTypeBuilder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            entityTypeBuilder.Property(x => x.Description).HasMaxLength(500);
        }

        public Tag()
        {
            Articles = new List<ArticleTag>();
        }
    }
}
