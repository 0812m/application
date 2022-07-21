using Microsoft.EntityFrameworkCore;
using PH.DatabaseAccessor.Entity;
using PH.DatabaseAccessor.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.DatabaseAccessor.Repository
{
    public interface IEFCoreRepositoryFactory:IRepositoryFactory
    {
        /// <summary>
        /// 指定上下文获取只读仓储
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDbContext"></typeparam>
        /// <returns></returns>
        IReadOnlyRepository<TEntity> GetReadOnlyRepository<TEntity, TDbContext>() where TEntity : Entity<TEntity> where TDbContext : DbContext;

        /// <summary>
        /// 指定上下文获取仓储
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDbContext"></typeparam>
        /// <returns></returns>
        IReadOnlyRepository<TEntity> GetRepository<TEntity, TDbContext>() where TEntity : Entity<TEntity> where TDbContext : DbContext;
    }
}
