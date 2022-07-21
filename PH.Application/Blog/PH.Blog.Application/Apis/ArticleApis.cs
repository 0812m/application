using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PH.Blog.Contract;
using PH.Blog.Contract.Dtos;
using PH.Blog.Contract.IApis;
using PH.Blog.Contract.IServices;
using PH.Blog.Entity;
using PH.Blog.Entity.Enum;
using PH.DatabaseAccessor.Entity;
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
    [AuthorizeApp, Route("api/[controller]"), ApiDescriptionSettings("文章模块")]
    public class ArticleApis : BaseApiController, IArticleApis
    {
        private readonly IArticleSvc _articleSvc;
        private readonly ILogSvc _logSvc;

        public ArticleApis(IHttpContextAccessor httpContextAccessor, IArticleSvc articleSvc, ILogSvc logSvc, IConfigurtionSvc configurtionSvc, ICacheProvide cache)
        : base(configurtionSvc,httpContextAccessor, cache)
        {
            _articleSvc = articleSvc;
            _logSvc = logSvc;
        }

        /// <summary>
        /// 新增文章
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("add"),SecurityDefinition(SecurityConst.ARTICLE_ADD)]
        public async Task AddAsync([FromBody] AddArticleInput input)
        {
           var entity = await _articleSvc.Add(new Article()
            {
                Pageviews = 0,
                Title = input.Title,
                Cover = input.Cover,
                Opening = input.Opening,
                CreateTime = DateTime.Now,
                Status = (ArticleStatus)input.Status,
                ReleaseTime = input.Status == 0 ? DateTime.Now : null,
                Content = new Content()
                {
                    Markdown = input.Markdown,
                    Text = input.Text,
                },
                Tags = input.Tags.Select(x => new ArticleTag
                {
                    TagId = x
                }).ToList()
            });

            await _logSvc.AddEvent(new Events()
            {
                Name = $"发布文章",
                Content = entity.Title,
                RelatedId = entity.Id,
                Type = (int)EventType.PostArticle
            });
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [Route("cmnt/add"),AllowAnonymous]
        public async Task AddCommentAsync([FromBody] AddCommentInput input)
        {
            var ip = GetRemoteIP();
            if (string.IsNullOrWhiteSpace(ip))
                throw Sorry.Bad(ErrorCodes.IPAddressAbnormal);

            var cacheKey = $"{ConstPool.CACHKKEY_COMMENT_PREFIX}{ip}";
            var ipCache = await _cache.GetAsync<string>(cacheKey);
            if (!string.IsNullOrWhiteSpace(ipCache))
                throw Sorry.Bad(ErrorCodes.CommentIntervalFrequent);

            var article = await _articleSvc.Get(input.ArticleId);
            if (article is null)
                throw Sorry.Bad(ErrorCodes.ArticleNotExist);

            var entity = new Comment()
            {
                ArticleId = input.ArticleId,
                Contact = input.Contact,
                Content = input.Content,
                NickName = input.NickName,
                IP = ip,
                CreateTime = DateTime.Now,
            };

              var defaultUser = await GenerateNickName();
            if (string.IsNullOrWhiteSpace(entity.NickName))
                entity.NickName = defaultUser.nickName;
            entity.Avatar = defaultUser.avatar;

            entity = await _articleSvc.Add(entity);
            await _cache.SaveAsync(cacheKey, ip, 60);
            await _logSvc.AddEvent(new Events()
            {
                Name = $"【{entity.NickName}】在【{article.Title}】文章留下了评论",
                Content = entity.Content,
                RelatedId = entity.Id,
                Type = (int)EventType.PostComment
            });
        }

        /// <summary>
        /// 获取所有【标签】
        /// </summary>
        /// <returns></returns>
        [Route("tag/all"), AllowAnonymous, ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)]
        public async Task<IEnumerable<TagDto>> GetAllTagAsync()
        {
            var tags = await _articleSvc.GetAllTagAsync();
            return tags.Select(t => new TagDto()
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description
            });
        }

        /// <summary>
        /// 通过id获取文章详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("get"), AllowAnonymous]
        public async Task<ArticleDetailDto> GetArticleByIdAsync([FromQuery] GetArticleByIdInput input)
        {
            var article = await _articleSvc.Get(input.Id);
            if (article is null)
                throw Sorry.Bad(ErrorCodes.ArticleNotExist);

            var tags = await _articleSvc.GetAllTagAsync();

           var source = GetSource();
            if (!string.IsNullOrWhiteSpace(source) && source.ToLower().Equals("client"))
            {
                article.Pageviews++;
               await _articleSvc.EditAsync(article);
            }

            return new ArticleDetailDto()
            {
                Id = article.Id,
                Title = article.Title,
                Pageviews = article.Pageviews,
                ReleaseTime = article.ReleaseTime.Value,
                MarkDownText = article.Content?.Markdown,
                Tags = string.Join('、', article.Tags.Select(t => tags.FirstOrDefault(f => f.Id == t.TagId)?.Name))
            };
        }

        /// <summary>
        /// 获取文章评论列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [Route("cmnt/get"), AllowAnonymous]
        public async Task<IEnumerable<CommentDto>> GetArticleOfCommentsAsync(int id)
        {
            return (await _articleSvc.GetCommentsByArticleIdAsync(id)).Select(x => new CommentDto()
            {
                NickName = x.NickName,
                Content = x.Content,
                CreateTime = x.CreateTime,
                Avatar = x.Avatar
            });
        }

        /// <summary>
        /// 文章搜索
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("search"), AllowAnonymous]
        public async Task<PageOutput<SearchArticleDto>> SearchArticleAsync([FromQuery] SearchArticleInput input)
        {
            var paged = await _articleSvc.SearchArticleAsync(input.SearchKey, input.Tags, input.PageIndex, input.PageSize);
            var tags = await _articleSvc.GetAllTagAsync();

            var items = paged.Items.Select(x => new SearchArticleDto()
            {
                Id = x.Id,
                Cover = x.Cover,
                Opening = x.Opening,
                Pageviews = x.Pageviews,
                ReleaseTime = x.ReleaseTime.Value.ToString("yyyy-MM-dd"),
                Title = x.Title,
                Tags = x.Tags.Select(t => tags.FirstOrDefault(f => f.Id == t.TagId)?.Name)
            });

            return new PageOutput<SearchArticleDto>()
            {
                Items = items,
                Total = paged.Total,
                PageIndex = input.PageIndex,
                PageSize = input.PageSize,
            };
        }
    }
}
