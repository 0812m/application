using PH.Blog.Contract.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;
using PH.Core.IOC.Attributes;
using PH.Blog.Entity;
using PH.DatabaseAccessor.Repository.Interfaces;

namespace PH.Blog.Application.ServiceImpl
{
    [AutoInjection]
    public class LinkSvc : ILinkSvc
    {
        private readonly IRepository<Links> _linksRepo;

        public LinkSvc
            (IRepository<Links> linksRepo)
        {
            _linksRepo = linksRepo;
        }

        /// <summary>
        /// 获取所有友链
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Links>> GetAllAsync()
        {
            return await _linksRepo.Where(x => !x.IsDeleted).OrderBy(x => x.Order).ToListAsync();
        }
    }
}
