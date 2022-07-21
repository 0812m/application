using PH.Blog.Contract.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.IApis
{
    public interface IMetaApis
    {
        /// <summary>
        /// 获取事件
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<EventDto>> GetEventsAsync();

        /// <summary>
        /// 获取网站数据
        /// </summary>
        /// <returns></returns>
        Task<WebsiteDataDto> GetWebsiteDataAsync();

        /// <summary>
        /// 添加访问日志
        /// </summary>
        /// <returns></returns>
        Task AcessLogAsync();

        /// <summary>
        /// 获取基础信息
        /// </summary>
        /// <returns></returns>
        Task<BasicInfoDto> GetBasicInfoAsync();
    }
}
