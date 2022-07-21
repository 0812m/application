using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PH.DatabaseAccessor;

namespace PH.EntityFrameworkCore
{
    [AppDbContext(DbProvide.MySQL, "ConnectionStrings")]
    public class DefaultDbContext : AppDbContext<DefaultDbContext>
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> dbContext) : base(dbContext)
        {
        }
    }
}
