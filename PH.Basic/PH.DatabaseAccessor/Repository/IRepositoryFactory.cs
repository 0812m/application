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
    /// <summary>
    /// 仓储工厂
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        /// 获取读写仓储
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity<TEntity>;

        /// <summary>
        /// 获取只读仓储
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IReadOnlyRepository<TEntity> GetReadOnlyRepository<TEntity>() where TEntity : Entity<TEntity>;
    }
}
