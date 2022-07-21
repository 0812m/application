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
    public class RolePrivilege: Entity<RolePrivilege>
    {
        public virtual int RoleId { get; set; }

        public virtual int PrivilegeId { get; set; }

        protected override void Mapping(EntityTypeBuilder<RolePrivilege> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("role_privilege");
            entityTypeBuilder.HasKey(x => new { x.RoleId, x.PrivilegeId });

            //entityTypeBuilder.HasOne<Privilege>().WithMany(x => x.RolePrivileges).HasForeignKey(x => x.PrivilegeId);
            //entityTypeBuilder.HasOne<Role>().WithMany(x => x.RolePrivileges).HasForeignKey(x => x.RoleId);
        }
    }
}
