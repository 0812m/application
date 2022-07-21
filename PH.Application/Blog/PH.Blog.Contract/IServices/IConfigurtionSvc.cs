using PH.Blog.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.IServices
{
    public interface IConfigurtionSvc
    {
        /// <summary>
        /// 通过 key 获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<Configurtion> GetAsync(string key);

        /// <summary>
        /// 通过 type 获取
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<IEnumerable<Configurtion>> GetByTypeAsync(string type);
    }
}
