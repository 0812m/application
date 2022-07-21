using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.DatabaseAccessor.Repository
{
    public partial class EFCoreRepository<TEntity>
    {
        public TEntity Update(TEntity entity)
        {
            return Entitys.Update(entity).Entity;
        }

        public void Update(IEnumerable<TEntity> entity)
        {
            Entitys.UpdateRange(entity);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            return await Task.FromResult(Update(entity));
        }

        public Task UpdateAsync(IEnumerable<TEntity> entity)
        {
            Update(entity);
            return Task.CompletedTask;
        }

        public TEntity UpdateNow(TEntity entity)
        {
            var savedEntity = Update(entity);
            SaveNow();
            return savedEntity;
        }

        public async Task<TEntity> UpdateNowAsync(TEntity entity)
        {
            var savedEntity = await UpdateAsync(entity);
            await SaveNowAsync();
            return savedEntity;
        }
    }
}
