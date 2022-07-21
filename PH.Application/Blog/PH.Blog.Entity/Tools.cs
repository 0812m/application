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
    public class Tools:CanDelEntity<Tools,int>
    {
        public virtual string Name { get; set; }

        public virtual string Image { get; set; }

        public virtual string? Description { get; set; }

        public virtual string DownloadUrl { get; set; }

        public virtual bool CanDownload { get; set; }

        protected override void Mapping(EntityTypeBuilder<Tools> entityTypeBuilder)
        {
            entityTypeBuilder.Property(x => x.IsDeleted).HasColumnName("deleted");
        }
    }
}
