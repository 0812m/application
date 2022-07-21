using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.DatabaseAccessor.Repository
{
    public partial class EFCoreRepository<TEntity>
    {
        public TEntity Insert(TEntity entity)
        {
            return Entitys.Add(entity).Entity;
        }

        public TEntity InsertNow(TEntity entity)
        {
            var inserted = Insert(entity);
            SaveNow();
            return inserted;
        }

        public void Insert(IEnumerable<TEntity> entitys)
        {
            Entitys.AddRange(entitys);
        }

        public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var task = await Entitys.AddAsync(entity, cancellationToken);
            return task.Entity;
        }

        public async Task<TEntity> InsertNowAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var inserted = await Task.FromResult(InsertNow(entity));
            await SaveNowAsync(cancellationToken);
            return inserted;
        }

        public async Task InsertAsync(IEnumerable<TEntity> entitys, CancellationToken cancellationToken = default)
        {
            await Entitys.AddRangeAsync(entitys, cancellationToken);
        }

        public void InsertNow(IEnumerable<TEntity> entitys)
        {
            Insert(entitys);
            SaveNow();
        }

        public async Task InsertNowAsync(IEnumerable<TEntity> entity, CancellationToken cancellationToken = default)
        {
            await InsertAsync(entity, cancellationToken);
            await SaveNowAsync(cancellationToken);
        }
    }
}
