using PH.Web.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.Dtos
{
    public class GetArticleByIdInput:Input
    {
        public virtual int Id { get; set; }
    }
}
