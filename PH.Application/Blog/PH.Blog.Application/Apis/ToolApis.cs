using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PH.Blog.Contract;
using PH.Blog.Contract.Dtos;
using PH.Blog.Contract.IApis;
using PH.Blog.Contract.IServices;
using PH.Blog.Entity.Enum;
using PH.Blog.Entity;
using PH.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Web.Core.Cache;
using PH.Web.Core.Authentication;

namespace PH.Blog.Application.Apis
{
    [AuthorizeApp,Route("api/[controller]"), ApiDescriptionSettings("工具模块")]
    public class ToolApis : BaseApiController, IToolApis
    {
        private readonly IToolSvc _toolSvc;
        private readonly ILogSvc _logSvc;

        public ToolApis(IToolSvc toolSvc, ILogSvc logSvc, IConfigurtionSvc configurtionSvc, ICacheProvide cache, IHttpContextAccessor httpContextAccessor)
            : base(configurtionSvc, httpContextAccessor, cache)
        {
            _toolSvc = toolSvc;
            _logSvc = logSvc;
        }

        /// <summary>
        /// 添加工具
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [Route("add"),SecurityDefinition(SecurityConst.TOOL_Add)]
        public async Task AddAsync(AddToolsInput input)
        {
           var entity = await _toolSvc.AddAsync(new Entity.Tools()
            {
                Name = input.Name,
                Description = input.Description,
                DownloadUrl = input.DownloadUrl,
                Image = input.Image,
                CanDownload  = input.CanDownload
            });

            await _logSvc.AddEvent(new Events()
            {
                Name = $"分享了工具 ",
                Content = entity.Name,
                RelatedId = entity.Id,
                Type = (int)EventType.UploadFile
            });
        }

        /// <summary>
        /// 获取所有工具
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [Route("all")]
        public async Task<IEnumerable<ToolsDto>> GetAllAsync()
        {
            var tools = await _toolSvc.GetAllAsync();
            return tools.Select(x => new ToolsDto()
            {
                Name = x.Name,
                Url = x.DownloadUrl,
                Image = x.Image,
                Description = x.Description,
                CanDownload = x.CanDownload
            });
        }
    }
}
