using PH.Blog.Entity;
using PH.Web.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.IServices
{
    public interface IMessageBoardSvc
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PageOutput<object>> GetToPageAsync(int pageIndex = 1,int pageSize = 20);

        /// <summary>
        /// 留言
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<MessageBoard> Add(MessageBoard entity);

        /// <summary>
        /// 统计留言
        /// </summary>
        /// <returns></returns>
        Task<int> CountAsync();
    }
}
