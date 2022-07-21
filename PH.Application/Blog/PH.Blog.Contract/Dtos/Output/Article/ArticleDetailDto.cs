using PH.Web.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.Dtos
{
    public class ArticleDetailDto:Dto
    {
        public virtual int Id { get; set; }

        public virtual string Title{ get; set; }

        public virtual string Tags { get; set; }

        public virtual int Pageviews { get; set; }

        public virtual DateTime ReleaseTime { get; set; }

        public virtual string MarkDownText { get; set; }
    }
}
