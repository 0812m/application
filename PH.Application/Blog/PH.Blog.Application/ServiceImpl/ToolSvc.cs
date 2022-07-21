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
    public class ToolSvc : IToolSvc
    {
        private readonly IRepository<Tools> _toolsRepo;

        public ToolSvc(IRepository<Tools> toolsRepo)
        {
            _toolsRepo = toolsRepo;
        }

        /// <summary>
        /// 添加工具
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Tools> AddAsync(Tools entity)
        {
          return await _toolsRepo.InsertNowAsync(entity);
        }

        /// <summary>
        /// 获取所有工具实体
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<Tools>> GetAllAsync()
        {
            return await _toolsRepo.Where(x => !x.IsDeleted).ToListAsync();
        }
    }
}
