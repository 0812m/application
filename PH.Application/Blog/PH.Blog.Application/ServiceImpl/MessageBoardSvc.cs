using PH.Blog.Contract.IServices;
using PH.Blog.Entity;
using PH.Web.Core.Contracts;
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
    public class MessageBoardSvc : IMessageBoardSvc
    {
        private readonly IRepository<MessageBoard> _mbRepo;

        public MessageBoardSvc(IRepository<MessageBoard> mbRepo)
        {
            _mbRepo = mbRepo;
        }

        /// <summary>
        /// 留言
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<MessageBoard> Add(MessageBoard entity)
        {
           return await _mbRepo.InsertNowAsync(entity);
        }

        /// <summary>
        /// 留言数量
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<int> CountAsync()
        {
           return await _mbRepo.CountAsync();
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PageOutput<object>> GetToPageAsync(int pageIndex = 1, int pageSize = 20)
        {
            var paged = await _mbRepo.Where(x => !x.IsDeleted).OrderByDescending(x => x.CreateTime).ToPagedAsync(pageIndex, pageSize, (x, index) => new
            {
                x.NickName,
                x.Content,
                x.Avatar,
                x.CreateTime
            });

            return new PageOutput<object>()
            {
                Items = paged.Items,
                Total = paged.Total,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }
    }
}
