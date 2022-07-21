using PH.Web.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.Dtos
{
    public class MessageBoardInput:Input
    {
        /// <summary>
        /// 内容
        /// </summary>
        public virtual string Content { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public virtual string Contact { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public virtual string NickName { get; set; }

        public override void Verification()
        {
            if (string.IsNullOrWhiteSpace(Content))
                throw Sorry.Wocao(ErrorCodes.CommentOfContentEmpty);
        }
    }
}
