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
    public class Links : CanDelEntity<Links, int>
    {
        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// url
        /// </summary>
        public virtual string Url { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public virtual int Order { get; set; }

        protected override void Mapping(EntityTypeBuilder<Links> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);

            entityTypeBuilder.Property(x => x.Description).HasMaxLength(250);
            entityTypeBuilder.Property(x => x.Url).HasMaxLength(250).IsRequired();
            entityTypeBuilder.Property(x => x.Order);
            entityTypeBuilder.Property(x => x.IsDeleted).HasColumnName("deleted");
            entityTypeBuilder.Property(x => x.CreateTime);
        }
    }
}
