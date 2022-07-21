using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Web.Core.Contracts;

namespace PH.Blog.Contract
{
    public enum ErrorCodes
    {
        [ErrorMetaData("文章标题必填！")]
        ArticleTitleIsRequired = 1001,
        [ErrorMetaData("文章封面必填！")]
        ArticleCoverIsRequired = 1002,
        [ErrorMetaData("文章开篇段落必填！")]
        ArticleOpeningIsRequired = 1003,
        [ErrorMetaData("文章内容必填！")]
        ArticleTextIsRequired = 1004,
        [ErrorMetaData("文章Markdown必填！")]
        ArticleMarkdownIsRequired = 1005,
        [ErrorMetaData("文章标签至少选一个！")]
        ArticleTagsIsRequired = 1006,
        [ErrorMetaData("文章不存在！")]
        ArticleNotExist = 1007,
        [ErrorMetaData("评论内容为空！")]
        CommentOfContentEmpty = 1008,
        [ErrorMetaData("IP地址异常！")]
        IPAddressAbnormal = 1009,
        [ErrorMetaData("说慢点说慢点")]
        CommentIntervalFrequent = 1010,
        [ErrorMetaData("文件丢失")]
        FileNotExist = 1011,
        [ErrorMetaData("文件分片异常")]
        FilePartialAbnormal = 1012,
        [ErrorMetaData("用户不存在")]
        UserNotExist = 1013,
        [ErrorMetaData("账号或密码有误！")]
        AccountOrPasswordIsIncorrect = 1014,
        [ErrorMetaData("账号不可用！")]
        InvalidUser = 1015
    }
}
