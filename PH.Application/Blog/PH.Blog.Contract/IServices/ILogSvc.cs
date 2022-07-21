using PH.Blog.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.IServices
{
    public interface ILogSvc
    {
        /// <summary>
        /// 新增事件
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        Task AddEvent(Events events);

        /// <summary>
        /// 获取所有事件
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Events>> GetAllEventsAsync();

        /// <summary>
        /// 访问数
        /// </summary>
        /// <returns></returns>
        Task<int> AcessCountAsync();

        /// <summary>
        /// 访问日志
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AcessLogs> AddAsync(AcessLogs entity);
    }
}
