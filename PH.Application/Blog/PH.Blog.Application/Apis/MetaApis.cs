using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PH.Blog.Contract;
using PH.Blog.Contract.Dtos;
using PH.Blog.Contract.IApis;
using PH.Blog.Contract.IServices;
using PH.Web.Core;
using PH.Web.Core.Authentication;
using PH.Web.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Application.Apis
{
    [AuthorizeApp,Route("api/[controller]"), ApiDescriptionSettings("元数据模块")]
    public class MetaApis : BaseApiController, IMetaApis
    {
        private readonly ILogSvc _logSvc;
        private readonly IArticleSvc _articleSvc;
        private readonly IMessageBoardSvc _messageBoardSvc;

        public MetaApis(ILogSvc logSvc, IConfigurtionSvc configurtionSvc, IArticleSvc articleSvc, IMessageBoardSvc messageBoardSvc, ICacheProvide cache, IHttpContextAccessor httpContextAccessor)
            : base(configurtionSvc, httpContextAccessor, cache)
        {
            _logSvc = logSvc;
            _articleSvc = articleSvc;
            _messageBoardSvc = messageBoardSvc;
        }

        /// <summary>
        /// 添加访问日志
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [Route("acess/log"), AllowAnonymous]
        public async Task AcessLogAsync()
        {
            await _logSvc.AddAsync(new Entity.AcessLogs()
            {
                IP = GetRemoteIP() ?? ""
            });
        }

        /// <summary>
        /// 获取作者基本信息
        /// </summary>
        /// <returns></returns>
        [Route("info"), AllowAnonymous]
        public async Task<BasicInfoDto> GetBasicInfoAsync()
        {
          var basicInfo = await _cache.GetAsync(ConstPool.CACHKKEY_BASICINFO,  () =>
             {
                 var configurtions =  _configurtionSvc.GetByTypeAsync(ConstPool.TYPE_BASICINFO).Result;
                 return new BasicInfoDto()
                 {
                     Avatar = configurtions.FirstOrDefault(x => x.Key == "Avatar")?.Value,
                     Description = configurtions.FirstOrDefault(x => x.Key == "Description")?.Value,
                     NickName = configurtions.FirstOrDefault(x => x.Key == "NickName")?.Value,
                     Email = configurtions.FirstOrDefault(x => x.Key == "Email")?.Value
                 };
             }, 60 * 120);

            var platforms = await _cache.GetAsync(ConstPool.CACHKKEY_PLATFORM,  () =>
            {
                var configurtions =  _configurtionSvc.GetByTypeAsync(ConstPool.TYPE_PLATFORM).Result;
                return configurtions.Select(x => x.Value?.Deserialize<PlatformDto>());
            }, 60 * 120);

            basicInfo.Platforms = platforms;

            return basicInfo;
        }

        /// <summary>
        /// 获取所有事件
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [Route("events/all"), AllowAnonymous, ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)]
        public async Task<IEnumerable<EventDto>> GetEventsAsync()
        {
            var events = await _logSvc.GetAllEventsAsync();
            return events.Select(e => new EventDto()
            {
                Event = e.Name,
                Content = e.Content,
                CreateTime = e.CreateTime
            });
        }

        /// <summary>
        /// 网站数据
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [Route("web/data"), AllowAnonymous, ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)]
        public async Task<WebsiteDataDto> GetWebsiteDataAsync()
        {
            var articleCount = await _articleSvc.CountAsync();
            var tagCount = (await _articleSvc.GetAllTagAsync()).Count();
            var messageCount = await _messageBoardSvc.CountAsync();
            var acessCount = await _logSvc.AcessCountAsync();

            return new WebsiteDataDto()
            {
                ArticleCount = articleCount,
                MessageCount = messageCount,
                TagCount = tagCount,
                Pageviews = acessCount
            };
        }
    }
}
