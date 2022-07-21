using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PH.Blog.Entity.Enum;
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
    /// 文章
    /// </summary>
    public class Article : BaseEntity<Article, int>
    {
        /// <summary>
        /// 标题
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public virtual string Cover { get; set; }

        /// <summary>
        /// 开头段落
        /// </summary>
        public virtual string Opening { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual ArticleStatus Status { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public virtual DateTime? ReleaseTime { get; set; }

        /// <summary>
        /// 浏览量
        /// </summary>
        public virtual int Pageviews { get; set; }

        /// <summary>
        /// 是否已删除
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public virtual Content Content { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public virtual IEnumerable<ArticleTag>  Tags { get; set; }

        /// <summary>
        /// 评论
        /// </summary>
        public virtual IEnumerable<Comment> Comments { get; set; }

        protected override void Mapping(EntityTypeBuilder<Article> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Title).HasMaxLength(250).IsRequired().HasColumnName("title");
            entityTypeBuilder.Property(x => x.Cover).HasMaxLength(250).IsRequired().HasColumnName("cover");
            entityTypeBuilder.Property(x => x.Opening).HasMaxLength(500).IsRequired().HasColumnName("opening");
            entityTypeBuilder.Property(x => x.Status).HasColumnName("status");
            entityTypeBuilder.Property(x => x.CreateTime).HasColumnName("createTime");
            entityTypeBuilder.Property(x => x.ReleaseTime).HasColumnName("releaseTime");
            entityTypeBuilder.Property(x => x.Pageviews).HasColumnName("pageviews");
            entityTypeBuilder.Property(x => x.IsDeleted).HasColumnName("deleted");
        }

        public Article()
        {
            Tags = new List<ArticleTag>();
            Comments = new List<Comment>();
        }
    }
}
