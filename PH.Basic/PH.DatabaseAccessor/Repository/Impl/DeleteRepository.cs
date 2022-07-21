using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PH.DatabaseAccessor.Repository
{
    public partial class EFCoreRepository<TEntity>
    {
        public TEntity Delete(TEntity entity)
        {
            return Entitys.Remove(entity).Entity;
        }

        public void Delete(IEnumerable<TEntity> entitys)
        {
            Entitys.RemoveRange(entitys);
        }

        public void Delete(Expression<Func<TEntity, bool>> expression)
        {
            Entitys.RemoveRange(Where(expression));
        }

        public TEntity DeleteNow(TEntity entity)
        {
            entity = Delete(entity);
            SaveNow();
            return entity;
        }

        public void DeleteNow(IEnumerable<TEntity> entitys)
        {
            Delete(entitys);
            SaveNow();
        }

        public Task<TEntity> DeleteAsync(TEntity entity)
        {
            return Task.FromResult(Delete(entity));
        }

        public Task DeleteAsync(IEnumerable<TEntity> entitys)
        {
            Delete(entitys);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Expression<Func<TEntity, bool>> expression)
        {
            Delete(expression);
            return Task.CompletedTask;
        }

        public async Task<TEntity> DeleteNowAsync(TEntity entity)
        {
            var deleted = await DeleteAsync(entity);
            await SaveNowAsync();
            return deleted;
        }

        public async Task DeleteNowAsync(IEnumerable<TEntity> entitys)
        {
            await DeleteAsync(entitys);
            await SaveNowAsync();
        }
    }
}
