using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.DatabaseAccessor
{
    public class DbContextPool : IDbContextPool
    {
        private readonly ConcurrentDictionary<Guid, DbContext> dbContexts;

        private readonly ConcurrentDictionary<Guid, DbContext> failedDbContexts;

        private readonly IServiceProvider serviceProvider;

        public DbContextPool(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            dbContexts = new ConcurrentDictionary<Guid, DbContext>();
            failedDbContexts = new ConcurrentDictionary<Guid, DbContext>();
        }

        /// <summary>
        /// 数据库上下文事务
        /// </summary>
        public IDbContextTransaction DbContextTransaction { get; private set; }

        public void AddToPool(DbContext dbContext)
        {
            var instanceId = dbContext.ContextId.InstanceId;

            var canAdd = dbContexts.TryAdd(instanceId, dbContext);
            if (canAdd)
            {
                dbContext.SaveChangesFailed += (s, e) =>
                {
                    canAdd = failedDbContexts.TryAdd(instanceId, dbContext);
                    if (canAdd)
                    {
                        var context = s as DbContext;
                        var database = context.Database;
                        var trans = database.CurrentTransaction;
                        //如果事务开启则回滚事务
                        if (trans != null)
                            trans.Rollback();
                    }
                };
            }
        }

        public Task AddToPoolAsync(DbContext dbContext)
        {
            AddToPool(dbContext);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <param name="ensureTransaction"></param>
        public void BeginTransaction(bool ensureTransaction = false)
        {
            if (!ensureTransaction) return;
            if (dbContexts.Any())
            {
                if (DbContextTransaction == null)
                {
                    var transactionDbContext = dbContexts.FirstOrDefault(x => x.Value.Database.CurrentTransaction != null);
                    if (transactionDbContext.Value != null)
                        DbContextTransaction = transactionDbContext.Value.Database.CurrentTransaction;
                    else
                        DbContextTransaction = dbContexts.First().Value.Database.BeginTransaction();
                }

                dbContexts.Where(x => x.Value != null && x.Value.Database.CurrentTransaction == null).Select(x => x.Value.Database.UseTransaction(DbContextTransaction.GetDbTransaction()));
            }
        }

        public void CloseAll()
        {
            if (dbContexts.Any())
            {
                foreach (var dbContext in dbContexts)
                {
                    var conn = dbContext.Value.Database.GetDbConnection();
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }
            }
        }

        public void CommitTransaction(bool isManualSaveChanges = true, Exception exception = null, bool withCloseAll = false)
        {
            if (exception == null)
            {
                try
                {
                    SavePoolNow();
                    DbContextTransaction?.Commit();
                }
                catch (Exception ex)
                {
                    if (DbContextTransaction?.GetDbTransaction()?.Connection != null)
                        DbContextTransaction.Rollback();
                    throw ex;
                }
            }
            else
            {
                if (DbContextTransaction?.GetDbTransaction()?.Connection != null)
                    DbContextTransaction.Rollback();
            }
            DbContextTransaction?.Dispose();
            DbContextTransaction = null;

            if (withCloseAll)
                CloseAll();
        }

        public ConcurrentDictionary<Guid, DbContext> GetDbContexts()
        {
            return dbContexts;
        }

        public int SavePoolNow()
        {
            if(!dbContexts.Any()) return 0;

            int num = 0;
            var hasChangeDbContexts = dbContexts.Select(x => x.Value).Where(x => x.ChangeTracker.HasChanges());
            foreach (var dbContext in hasChangeDbContexts)
            {
                num += dbContext.SaveChanges();
            }
            return num;
        }

        public async Task<int> SavePoolNowAsync(CancellationToken cancellationToken = default)
        {
            return await SavePoolNowAsync(false,cancellationToken);
        }

        public async Task<int> SavePoolNowAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            if (!dbContexts.Any()) return 0;

            int num = 0;
            var hasChangeDbContexts = dbContexts.Select(x => x.Value).Where(x => x.ChangeTracker.HasChanges());
            foreach (var dbContext in hasChangeDbContexts)
            {
                num += await dbContext.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            }
            return num;
        }
    }
}
