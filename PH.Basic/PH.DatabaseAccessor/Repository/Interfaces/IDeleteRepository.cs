using PH.DatabaseAccessor.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PH.DatabaseAccessor.Repository.Interfaces
{
    public interface IDeleteRepository<TEntity>
        where TEntity : Entity<TEntity>
    {
        TEntity Delete(TEntity entity);
        void Delete(IEnumerable<TEntity> entitys);
        void Delete(Expression<Func<TEntity, bool>> expression);
        TEntity DeleteNow(TEntity entity);
        void DeleteNow(IEnumerable<TEntity> entitys);

        Task<TEntity> DeleteAsync(TEntity entity);
        Task DeleteAsync(IEnumerable<TEntity> entitys);
        Task DeleteAsync(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> DeleteNowAsync(TEntity entity);
        Task DeleteNowAsync(IEnumerable<TEntity> entitys);
    }
}
