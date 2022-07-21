using PH.Blog.Contract.IApis;
using PH.Blog.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PH.Web.Core;
using Microsoft.AspNetCore.Mvc;
using PH.Blog.Contract.IServices;
using PH.Blog.Contract.Dtos;
using PH.Web.Core.Cache;
using PH.Web.Core.Authentication;

namespace PH.Blog.Application.Apis
{
    [AuthorizeApp,Route("api/[controller]"), ApiDescriptionSettings("友链模块")]
    public class LinkApis : BaseApiController, ILinkApis
    {
        private readonly ILinkSvc _linkSvc;

        public LinkApis(IHttpContextAccessor httpContextAccessor, ILinkSvc linkSvc, IConfigurtionSvc configurtionSvc, ICacheProvide cache) 
        : base(configurtionSvc,httpContextAccessor,cache)
        {
            _linkSvc = linkSvc;
        }

        /// <summary>
        /// 获取所有友链
        /// </summary>
        /// <returns></returns>
        [Route("all")]
        public async Task<IEnumerable<GetAllLinksDto>> GetGetAllLinksAsync()
        {
          var links = await _linkSvc.GetAllAsync();
            return links.Select(x => new GetAllLinksDto() 
            {
                 Url = x.Url,
                 Description = x.Description,
            });
        }
    }
}
