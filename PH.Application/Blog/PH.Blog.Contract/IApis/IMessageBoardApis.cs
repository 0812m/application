using PH.Blog.Contract.Dtos;
using PH.Web.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.IApis
{
    public interface IMessageBoardApis
    {
        /// <summary>
        /// 留言
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task AddMessageBoardAsync(MessageBoardInput input);

        /// <summary>
        /// 获取留言并分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PageOutput<object>> GetToPagedAsync(GetToPagedInput input);
    }
}
