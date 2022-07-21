using PH.Blog.Entity;
using PH.Web.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.IServices
{
    public interface IArticleSvc
    {
        /// <summary>
        /// 获取所有文章标签
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Tag>> GetAllTagAsync();

        /// <summary>
        /// 文章搜索
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="tags"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PageOutput<Article>> SearchArticleAsync(string searchKey,IEnumerable<int> tags,int pageIndex = 1,int pageSize = 20);

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Article> Add(Article entity);

        /// <summary>
        /// 通过 id 获取文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Article> Get(int id);

        /// <summary>
        /// 通过 articleId 获取 comments
        /// </summary>
        /// <param name="articleId">文章id</param>
        /// <returns>该文章的所有评论</returns>
        Task<IEnumerable<Comment>> GetCommentsByArticleIdAsync(int articleId);

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Comment> Add(Comment entity);

        /// <summary>
        /// 文章数量
        /// </summary>
        /// <returns></returns>
        Task<int> CountAsync();

        /// <summary>
        /// 编辑文章
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Article> EditAsync(Article entity);
    }
}
