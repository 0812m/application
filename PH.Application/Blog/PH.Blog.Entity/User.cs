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
    public class User: CanDelEntity<User,int>
    {
        public virtual string Account { get; set; }

        public virtual string PasswordHash { get; set; }

        public virtual string NickName { get; set; }

        public virtual string Description { get; set; }

        public virtual IEnumerable<Role> Roles { get; set; }

        protected override void Mapping(EntityTypeBuilder<User> entityTypeBuilder)
        {
            entityTypeBuilder.HasMany(x => x.Roles).WithMany(x => x.Users).UsingEntity<UserRole>();
            entityTypeBuilder.Property(x => x.IsDeleted).HasColumnName("deleted");
        }

        public User()
        {
            Roles = new List<Role>();
        }
    }
}
