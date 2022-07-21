using PH.Blog.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.IServices
{
    public interface ILinkSvc
    {
        /// <summary>
        /// 获取所有友链
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Links>> GetAllAsync();
    }
}
