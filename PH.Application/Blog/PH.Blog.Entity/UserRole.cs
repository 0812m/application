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
    public class UserRole:Entity<UserRole>
    {
        public virtual int UserId { get; set; }

        public virtual int RoleId { get; set; }

        protected override void Mapping(EntityTypeBuilder<UserRole> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("user_role");
            entityTypeBuilder.HasKey(x => new { x.UserId, x.RoleId });

            //entityTypeBuilder.HasOne<User>().WithMany(x => x.UserRole).HasForeignKey(x => x.UserId);
            //entityTypeBuilder.HasOne<Role>().WithMany(x => x.UserRole).HasForeignKey(x => x.RoleId);
        }
    }
}
