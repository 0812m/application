using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.Dtos
{
    public class WebsiteDataDto
    {
        public virtual int Pageviews { get; set; }

        public virtual int ArticleCount { get; set; }

        public virtual int MessageCount { get; set; }

        public virtual int TagCount { get; set; }
    }
}
