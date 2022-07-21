using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PH.Blog.Contract;
using PH.Blog.Contract.Dtos;
using PH.Blog.Contract.IApis;
using PH.Blog.Contract.IServices;
using PH.Blog.Entity;
using PH.Blog.Entity.Enum;
using PH.Web.Core;
using PH.Web.Core.Authentication;
using PH.Web.Core.Cache;
using PH.Web.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Application.Apis
{
    [AuthorizeApp,Route("api/[controller]"), ApiDescriptionSettings("留言板模块")]
    public class MessageBoardApis : BaseApiController, IMessageBoardApis
    {
        private readonly IMessageBoardSvc _messageBoardSvc;
        private readonly ILogSvc _logSvc;

        public MessageBoardApis(IMessageBoardSvc messageBoardSvc, ILogSvc logSvc, IConfigurtionSvc configurtionSvc, ICacheProvide cache, IHttpContextAccessor httpContextAccessor)
            : base(configurtionSvc, httpContextAccessor, cache)
        {
            _messageBoardSvc = messageBoardSvc;
            _logSvc = logSvc;
        }

        /// <summary>
        /// 留言
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [Route(""), AllowAnonymous]
        public async Task AddMessageBoardAsync(MessageBoardInput input)
        {
            var defaultUser = await GenerateNickName();
            var ip = GetRemoteIP();
            var cacheKey = $"{ConstPool.CACHKKEY_MESSAGE_PREFIX}{ip}";
            var ipCache = await _cache.GetAsync<string>(cacheKey);
            if (!string.IsNullOrWhiteSpace(ipCache))
                throw Sorry.Bad(ErrorCodes.CommentIntervalFrequent);

            var entity = await _messageBoardSvc.Add(new MessageBoard()
            {
                Contact = input.Contact,
                Content = input.Content,
                CreateTime = DateTime.Now,
                IP = GetRemoteIP(),
                NickName = string.IsNullOrWhiteSpace(input.NickName) ? defaultUser.nickName : input.NickName,
                Avatar = defaultUser.avatar
            });

            await _logSvc.AddEvent(new Events()
            {
                Name = $"【{entity.NickName}】留了言",
                Content = entity.Content,
                RelatedId = entity.Id,
                Type = (int)EventType.Message
            });

            await _cache.SaveAsync(ip, cacheKey);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [Route("page"), AllowAnonymous]
        public async Task<PageOutput<object>> GetToPagedAsync([FromQuery] GetToPagedInput input)
        {
            return await _messageBoardSvc.GetToPageAsync(input.PageIndex, input.PageSize);
        }
    }
}
