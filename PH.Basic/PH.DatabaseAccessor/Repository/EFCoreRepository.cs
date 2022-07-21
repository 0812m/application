using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using PH.DatabaseAccessor.Entity;
using PH.DatabaseAccessor.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.DatabaseAccessor.Repository
{
    /// <summary>
    /// EFCore 只读仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public partial class ReadOnlyEFCoreRepository<TEntity> : IReadOnlyRepository<TEntity>
        where TEntity : Entity<TEntity>
    {
        public ReadOnlyEFCoreRepository(IServiceProvider serviceProvider, Type dbContextLocato = null)
        {
            dbContextLocato = dbContextLocato ?? typeof(SlaveContextLocator);
            var dbcontextFactoryFunc = serviceProvider.GetService<Func<Type, DbContext>>();
            DbContext = dbcontextFactoryFunc?.Invoke(dbContextLocato);
            Entitys = DbContext?.Set<TEntity>();
        }

        protected virtual DbContext DbContext { get; set; }

        protected virtual DbSet<TEntity> Entitys { get; set; }
    }

    /// <summary>
    /// EFCore 读写仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public partial class EFCoreRepository<TEntity> : ReadOnlyEFCoreRepository<TEntity>, IRepository<TEntity>
where TEntity : Entity<TEntity>
    {
        private DbContextPool _dbContextPool { get; set; }
        public EFCoreRepository(IServiceProvider serviceProvider) : base(serviceProvider, typeof(MasterContextLocator))
        {
            _dbContextPool = serviceProvider.GetService<DbContextPool>();
        }

        public void AcceptAllChanges()
        {
            DbContext.ChangeTracker.AcceptAllChanges();
        }

        public int SaveNow()
        {
            return DbContext.SaveChanges();
        }

        public int SaveNow(bool acceptAllChangesOnSuccess)
        {
            return DbContext.SaveChanges(acceptAllChangesOnSuccess);
        }

        public async Task<int> SaveNowAsync(CancellationToken cancellationToken = default)
        {
            return await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> SaveNowAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return await DbContext.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public int SavePoolNow()
        {
            return _dbContextPool.SavePoolNow();
        }

        public async Task<int> SavePoolNowAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContextPool.SavePoolNowAsync(cancellationToken);
        }

        public async Task<int> SavePoolNowAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return await _dbContextPool.SavePoolNowAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }

    /// <summary>
    /// EFCore 仓储工厂
    /// </summary>
    public class EFCoreRepositoryFactory : IEFCoreRepositoryFactory
    {
        private readonly IServiceProvider serviceProvider;

        public EFCoreRepositoryFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IReadOnlyRepository<TEntity> GetReadOnlyRepository<TEntity>() where TEntity : Entity<TEntity>
        {
           return new ReadOnlyEFCoreRepository<TEntity>(serviceProvider);
        }

        public IReadOnlyRepository<TEntity> GetReadOnlyRepository<TEntity, TDbContext>()
            where TEntity : Entity<TEntity>
            where TDbContext : DbContext
        {
            throw new NotImplementedException();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity<TEntity>
        {
            return new EFCoreRepository<TEntity>(serviceProvider);
        }

        public IReadOnlyRepository<TEntity> GetRepository<TEntity, TDbContext>()
            where TEntity : Entity<TEntity>
            where TDbContext : DbContext
        {
            throw new NotImplementedException();
        }
    }
}
