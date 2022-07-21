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
    public class Privilege:CanDelEntity<Privilege,int>
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual IEnumerable<Role>  Roles { get; set; }

        protected override void Mapping(EntityTypeBuilder<Privilege> entityTypeBuilder)
        {
            entityTypeBuilder.HasMany(x => x.Roles).WithMany(x => x.Privileges).UsingEntity<RolePrivilege>();
            entityTypeBuilder.Property(x => x.IsDeleted).HasColumnName("deleted");
        }

        public Privilege()
        {
            Roles = new List<Role>();
        }
    }
}
