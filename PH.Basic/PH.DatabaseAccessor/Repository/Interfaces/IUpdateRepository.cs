using PH.DatabaseAccessor.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.DatabaseAccessor.Repository.Interfaces
{
    public interface IUpdateRepository<TEntity>
        where TEntity : Entity<TEntity>
    {
        TEntity Update(TEntity entity);
        void Update(IEnumerable<TEntity> entity);

        Task<TEntity> UpdateAsync(TEntity entity);
        Task UpdateAsync(IEnumerable<TEntity> entity);

        TEntity UpdateNow(TEntity entity);
        Task<TEntity> UpdateNowAsync(TEntity entity);
    }
}
