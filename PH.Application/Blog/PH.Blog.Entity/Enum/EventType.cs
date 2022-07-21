using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Entity.Enum
{
    public enum EventType
    {
        /// <summary>
        /// 发布文章
        /// </summary>
        PostArticle,
        /// <summary>
        /// 评论
        /// </summary>
        PostComment,
        /// <summary>
        /// 留言
        /// </summary>
        Message,
        /// <summary>
        /// 上传文件
        /// </summary>
        UploadFile,
        /// <summary>
        /// 其他
        /// </summary>
        Other
    }
}
