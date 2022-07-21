using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.Dtos
{
    public class BasicInfoDto
    {
        /// <summary>
        /// 头像
        /// </summary>
        public virtual string Avatar { get; set; }

        public virtual string NickName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Description { get; set; }

        public virtual string Email { get; set; }

        /// <summary>
        /// 平台
        /// </summary>
        public virtual IEnumerable<PlatformDto> Platforms { get; set; }
    }

    public class PlatformDto
    {
        public virtual string Name { get; set; }

        public virtual string Image { get; set; }

        public virtual string Url { get; set; }
    }
}
