using PH.DatabaseAccessor.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.DatabaseAccessor.Repository.Interfaces
{
    public interface IInsertRepository<TEntity>
        where TEntity : Entity<TEntity>
    {
        TEntity Insert(TEntity entity);
        TEntity InsertNow(TEntity entity);
        void Insert(IEnumerable<TEntity> entitys);
        void InsertNow(IEnumerable<TEntity> entity);

        Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task InsertAsync(IEnumerable<TEntity> entitys, CancellationToken cancellationToken = default);
        Task<TEntity> InsertNowAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task InsertNowAsync(IEnumerable<TEntity> entity, CancellationToken cancellationToken = default);
    }
}
