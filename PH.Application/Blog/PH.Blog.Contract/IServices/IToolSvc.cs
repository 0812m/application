using PH.Blog.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.IServices
{
    public interface IToolSvc
    {
        /// <summary>
        /// 获取所有工具实体
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Tools>> GetAllAsync();

        /// <summary>
        /// 添加工具
        /// </summary>
        /// <returns></returns>
        Task<Tools> AddAsync(Tools entity);
    }
}
