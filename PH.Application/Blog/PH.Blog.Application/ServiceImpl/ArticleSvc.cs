using PH.Blog.Contract.IServices;
using PH.Blog.Entity;
using PH.DatabaseAccessor.Repository.Interfaces;
using PH.Web.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Core.IOC.Attributes;
using PH.Blog.Contract;
using PH.Web.Core.Contracts;
using Microsoft.EntityFrameworkCore;

namespace PH.Blog.Application.ServiceImpl
{
    [AutoInjection]
    public class ArticleSvc : IArticleSvc
    {
        private readonly IRepository<Tag> _tagRepo;
        private readonly IRepository<Article> _articleRepo;
        private readonly IRepository<Content> _contentRepo;
        private readonly IRepository<Comment> _commentRepo;
        private readonly IRepository<ArticleTag> _articleTagRepo;
        private readonly IRepository<Configurtion> _configurtionRepo;
        private readonly ICacheProvide _cacheProvide;

        public ArticleSvc
            (
            IRepository<Tag> tagRepo,
            IRepository<Article> articleRepo,
            IRepository<Content> contentRepo,
            IRepository<Comment> commentRepo,
            IRepository<ArticleTag> articleTagRepo,
            IRepository<Configurtion> configurtionRepo,
            ICacheProvide cacheProvide
            )
        {
            _tagRepo = tagRepo;
            _articleRepo = articleRepo;
            _contentRepo = contentRepo;
            _commentRepo = commentRepo;
            _articleTagRepo = articleTagRepo;
            _configurtionRepo = configurtionRepo;
            _cacheProvide = cacheProvide;
        }

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Article> Add(Article entity)
        {
           return await _articleRepo.InsertNowAsync(entity);
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Comment> Add(Comment entity)
        {
          return await _commentRepo.InsertNowAsync(entity);
        }

        /// <summary>
        /// 获取文章数量
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<int> CountAsync()
        {
            return await _articleRepo.CountAsync();
        }

        /// <summary>
        /// 编辑文章
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Article> EditAsync(Article entity)
        {
           return await _articleRepo.UpdateNowAsync(entity);
        }

        /// <summary>
        /// 通过id获取文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Article> Get(int id)
        {
          return await _articleRepo.Include(x => x.Tags).Include(x => x.Content).FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        /// <summary>
        /// 获取所有文章标签
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<Tag>> GetAllTagAsync()
        {
            return await _cacheProvide.GetAsync(ConstPool.CACHKKEY_TAG, () =>
               {
                   return _tagRepo.AsEnumerable();
               }, 60 * 120);
        }

        /// <summary>
        /// 通过 articleId 获取评论
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<Comment>> GetCommentsByArticleIdAsync(int articleId)
        {
           return await _commentRepo.Where(x => x.ArticleId == articleId).ToListAsync();
        }

        /// <summary>
        /// 文章搜索
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="tags"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PageOutput<Article>> SearchArticleAsync(string searchKey, IEnumerable<int> tags, int pageIndex = 1, int pageSize = 20)
        {
           var query = _articleRepo.Include(x => x.Tags).Where(x => !x.IsDeleted && x.Status == 0).AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchKey))
                query = query.Where(x => x.Title.Contains(searchKey));

            if (tags is not null && tags.Any(x => x > 0))
                query = query.Where(x => x.Tags.Any(t => tags.Contains(t.TagId)));

           var paged = await query.OrderByDescending(x => x.ReleaseTime).ToPagedAsync(pageIndex,pageSize);

            return new PageOutput<Article>() 
            {
                 Items = paged.Items,
                 Total = paged.Total,
                 PageIndex = pageIndex,
                 PageSize = pageSize
            };
        }
    }
}
