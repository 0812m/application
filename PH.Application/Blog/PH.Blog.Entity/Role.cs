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
    public class Role:CanDelEntity<Role,int>
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual IEnumerable<User> Users { get; set; }

        public virtual IEnumerable<Privilege> Privileges { get; set; }

        protected override void Mapping(EntityTypeBuilder<Role> entityTypeBuilder)
        {
            entityTypeBuilder.HasMany(x => x.Users).WithMany(x => x.Roles).UsingEntity<UserRole>();
            entityTypeBuilder.HasMany(x => x.Privileges).WithMany(x => x.Roles).UsingEntity<RolePrivilege>();
            entityTypeBuilder.Property(x => x.IsDeleted).HasColumnName("deleted");
        }

        public Role()
        {
            Users = new List<User>();
            Privileges = new List<Privilege>();
        }
    }
}
