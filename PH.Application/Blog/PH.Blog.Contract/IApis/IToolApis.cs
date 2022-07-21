using PH.Blog.Contract.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.IApis
{
    public interface IToolApis
    {
        /// <summary>
        /// 获取所有工具
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ToolsDto>> GetAllAsync();

        /// <summary>
        /// 添加工具
        /// </summary>
        /// <returns></returns>
        Task AddAsync(AddToolsInput input);
    }
}
