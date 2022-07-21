using PH.Blog.Contract.Dtos;
using PH.Blog.Entity;
using PH.Web.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.IApis
{
    public interface IArticleApis
    {
        /// <summary>
        /// 获取所有标签
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TagDto>> GetAllTagAsync();

        /// <summary>
        /// 新增文章
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task AddAsync(AddArticleInput input);

        /// <summary>
        /// 文章搜索
        /// </summary>
        /// <returns></returns>
        Task<PageOutput<SearchArticleDto>> SearchArticleAsync(SearchArticleInput input);

        /// <summary>
        /// 通过 id 获取文章详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ArticleDetailDto> GetArticleByIdAsync(GetArticleByIdInput input);

        /// <summary>
        /// 评论
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task AddCommentAsync(AddCommentInput input);

        /// <summary>
        /// 获取文章评论
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<CommentDto>> GetArticleOfCommentsAsync(int id);
    }
}
