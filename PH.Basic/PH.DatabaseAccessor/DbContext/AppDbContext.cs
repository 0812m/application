using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.ToolsLibrary.Reflection;
using PH.DatabaseAccessor.Entity;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PH.DatabaseAccessor
{
    public class AppDbContext<TDbContext> : AppDbContext<TDbContext, MasterContextLocator>
        where TDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<TDbContext> dbContext) : base(dbContext)
        {

        }
    }


    public class AppDbContext<TDbContext, TDbContextLocator> : DbContext
        where TDbContext : DbContext
        where TDbContextLocator : IDbContextLocator
    {
        public AppDbContext(DbContextOptions<TDbContext> dbContext) : base(dbContext)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //获取所有为实现 IEntity 且 可实例化的类型
            var entitys = ReflectionUtil.FindAllImplementClass<IEntity>(t => t.IsClass && !t.IsAbstract && !t.IsSealed);

            var method = typeof(ModelBuilder).GetMethod(nameof(modelBuilder.ApplyConfiguration));
            foreach (var entity in entitys)
            {
                var Instance = Activator.CreateInstance(entity);
                method.MakeGenericMethod(entity).Invoke(modelBuilder, new object[] { Instance });
            }
        }
    }
}
