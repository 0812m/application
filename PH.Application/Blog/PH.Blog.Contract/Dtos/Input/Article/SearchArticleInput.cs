using PH.Web.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.Dtos
{
    public class SearchArticleInput:PagedInput
    {
        public virtual string SearchKey { get; set; }

        public virtual int[] Tags { get; set; }
    }
}
