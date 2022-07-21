using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.Dtos
{
    public class CommentDto
    {
        public virtual string NickName { get; set; }

        public virtual string Content { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public virtual string Avatar { get; set; }

        public virtual DateTime CreateTime { get; set; }
    }
}
