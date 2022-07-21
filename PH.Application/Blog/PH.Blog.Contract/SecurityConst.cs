using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract
{
    /// <summary>
    /// Security 常量 
    /// 常量名：模块名_[ 功能 | route ]  全大写
    /// 值：api.模块名.[ 功能 | route ]   全小写
    /// </summary>
    public class SecurityConst
    {
        #region 文章模块
        public const string ARTICLE_ADD = "api.article.add";
        #endregion

        #region 用户模块
        #endregion

        #region 友链模块
        #endregion

        #region 媒体数据模块
        public const string MEDIA_UPLOAD_PART = "api.media.upload.part";
        public const string MEDIA_MERGE = "api.media.merge";
        public const string MEDIA_PART_INDEX = "api.media.part.index";
        #endregion

        #region 留言板模块
        #endregion

        #region 元数据模块
        #endregion

        #region 工具模块
        public const string TOOL_Add = "api.tool.add";
        #endregion
    }
}
