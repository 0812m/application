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
    public class Comment : CanDelEntity<Comment, int>
    {
        /// <summary>
        /// 文章id
        /// </summary>
        public virtual int ArticleId { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public virtual string Content { get; set; }

        /// <summary>
        /// 联系方式  仅限 email or phone
        /// </summary>
        public virtual string Contact { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public virtual string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public virtual string Avatar { get; set; }

        /// <summary>
        /// ip地址
        /// </summary>
        public virtual string IP { get; set; }

        /// <summary>
        /// 文章实体
        /// </summary>
        public virtual Article  Article { get; set; }

        protected override void Mapping(EntityTypeBuilder<Comment> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);

            entityTypeBuilder.HasOne(x => x.Article).WithMany(x => x.Comments).HasForeignKey(x => x.ArticleId);
            entityTypeBuilder.Property(x => x.Content).HasMaxLength(1000).IsUnicode().IsRequired();
            entityTypeBuilder.Property(x => x.Contact).HasMaxLength(100);
            entityTypeBuilder.Property(x => x.IP).HasMaxLength(50);
            entityTypeBuilder.Property(x => x.CreateTime);
            entityTypeBuilder.Property(x => x.IsDeleted).HasColumnName("deleted");
        }
    }
}
