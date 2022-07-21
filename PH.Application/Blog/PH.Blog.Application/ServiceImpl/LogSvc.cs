using PH.Blog.Contract.IServices;
using PH.Blog.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Core.IOC.Attributes;
using PH.DatabaseAccessor.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PH.Blog.Application.ServiceImpl
{
    [AutoInjection]
    public class LogSvc : ILogSvc
    {
        private readonly IRepository<Events> _eventsRepo;
        private readonly IRepository<AcessLogs> _acesslogsRepo;

        public LogSvc(IRepository<Events> eventsRepo, IRepository<AcessLogs> acesslogsRepo)
        {
            _eventsRepo = eventsRepo;
            _acesslogsRepo = acesslogsRepo;
        }

        /// <summary>
        /// 访问数
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<int> AcessCountAsync()
        {
            return await _acesslogsRepo.CountAsync();
        }

        /// <summary>
        /// 访问日志
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<AcessLogs> AddAsync(AcessLogs entity)
        {
            return await _acesslogsRepo.InsertAsync(entity);
        }

        /// <summary>
        /// 新增事件
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        public async Task AddEvent(Events events)
        {
            await _eventsRepo.InsertAsync(events);
        }

        /// <summary>
        /// 获取所有事件
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<Events>> GetAllEventsAsync()
        {
           return await _eventsRepo.AsQueryable().OrderByDescending(x => x.CreateTime).ToListAsync();
        }
    }
}
