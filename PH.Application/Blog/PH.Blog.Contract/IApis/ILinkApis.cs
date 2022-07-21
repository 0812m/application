using PH.Blog.Contract.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.IApis
{
   public interface ILinkApis
    {
        /// <summary>
        /// 获取所有友链
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<GetAllLinksDto>> GetGetAllLinksAsync();
    }
}
