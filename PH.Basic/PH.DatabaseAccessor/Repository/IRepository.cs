using Microsoft.EntityFrameworkCore;
using PH.DatabaseAccessor.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.DatabaseAccessor.Repository.Interfaces
{
    public interface IRepository<TEntity> :
        IUpdateRepository<TEntity>,
        IInsertRepository<TEntity>,
        IDeleteRepository<TEntity>,
        IReadOnlyRepository<TEntity>
        where TEntity : Entity<TEntity>
    {
        void AcceptAllChanges();

        int SaveNow();

        int SaveNow(bool acceptAllChangesOnSuccess);

        Task<int> SaveNowAsync(CancellationToken cancellationToken = default);

        Task<int> SaveNowAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

        int SavePoolNow();

        Task<int> SavePoolNowAsync(CancellationToken cancellationToken = default);

        Task<int> SavePoolNowAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
    }
}
