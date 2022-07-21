using PH.Web.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.Dtos
{
    /// <summary>
    /// add article model
    /// </summary>
    public class AddArticleInput:Input
    {
        /// <summary>
        /// 标题
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public virtual string Cover { get; set; }

        /// <summary>
        /// 开头段落
        /// </summary>
        public virtual string Opening { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual int Status { get; set; }

        /// <summary>
        /// 内容（无html）
        /// </summary>
        public virtual string Text { get; set; }

        /// <summary>
        /// markdown 用于渲染页面
        /// </summary>
        public virtual string Markdown { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public virtual int[] Tags { get; set; }

        public override void Verification()
        {
            if (string.IsNullOrWhiteSpace(Title))
                throw Sorry.Bad(ErrorCodes.ArticleTitleIsRequired);
            if (string.IsNullOrWhiteSpace(Cover))
                throw Sorry.Bad(ErrorCodes.ArticleCoverIsRequired);
            if (string.IsNullOrWhiteSpace(Opening))
                throw Sorry.Bad(ErrorCodes.ArticleOpeningIsRequired);
            if (string.IsNullOrWhiteSpace(Text))
                throw Sorry.Bad(ErrorCodes.ArticleTextIsRequired);
            if (string.IsNullOrWhiteSpace(Markdown))
                throw Sorry.Bad(ErrorCodes.ArticleMarkdownIsRequired);
            if (Tags is null || !Tags.Any(x => x >  0))
                throw Sorry.Bad(ErrorCodes.ArticleTagsIsRequired);
        }
    }
}
