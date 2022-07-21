using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PH.DatabaseAccessor.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Entity
{
    public class Configurtion:BaseEntity<Configurtion,int>
    {
        /// <summary>
        /// 类型
        /// </summary>
        public virtual string Type { get; set; }

        /// <summary>
        /// key
        /// </summary>
        public virtual string Key { get; set; }

        /// <summary>
        /// value
        /// </summary>
        public virtual string Value { get; set; }

        /// <summary>
        /// subvalue
        /// </summary>
        public virtual string SubValue { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public virtual string? Description { get; set; }

        protected override void Mapping(EntityTypeBuilder<Configurtion> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);

            entityTypeBuilder.Property(x => x.Type).HasMaxLength(100).IsRequired();  
            entityTypeBuilder.Property(x => x.Key).HasMaxLength(100).IsRequired();  
            entityTypeBuilder.Property(x => x.Value).HasMaxLength(500).IsRequired();  
            entityTypeBuilder.Property(x => x.Description).HasMaxLength(500);  
            entityTypeBuilder.Property(x => x.CreateTime);  
        }
    }
}
