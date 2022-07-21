using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract
{
    /// <summary>
    /// 常量池
    /// </summary>
    public static class ConstPool
    {
        #region 缓存key

        /// <summary>
        /// 标签缓存key
        /// </summary>
        public const string CACHKKEY_TAG = "ck_tags";

        /// <summary>
        /// 评论缓存前缀
        /// </summary>
        public const string CACHKKEY_COMMENT_PREFIX = "ck_comment_";

        /// <summary>
        /// 留言缓存前缀
        /// </summary>
        public const string CACHKKEY_MESSAGE_PREFIX = "ck_message_";

        /// <summary>
        /// 平台缓存
        /// </summary>
        public const string CACHKKEY_PLATFORM = "ck_platform";

        /// <summary>
        /// 基础信息缓存
        /// </summary>
        public const string CACHKKEY_BASICINFO = "ck_basicInfo";

        /// <summary>
        /// 权限缓存key
        /// </summary>
        public const string CACHEKEY_PRIVILEGE = "ck_privileges";
        #endregion

        #region Configuration 配置
        /// <summary>
        /// 作者基础信息
        /// </summary>
        public const string TYPE_BASICINFO = "BasicInfo";

        /// <summary>
        /// 平台
        /// </summary>
        public const string TYPE_PLATFORM = "Platform";

        /// <summary>
        /// 默认用户
        /// </summary>
        public const string TYPE_DEFAULT_USER = "DefaultUser";
        #endregion
    }
}
