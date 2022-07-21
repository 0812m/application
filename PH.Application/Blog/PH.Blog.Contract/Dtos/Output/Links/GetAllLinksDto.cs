using PH.Web.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.Dtos
{
    public class GetAllLinksDto:Dto
    {
        public virtual string Url { get; set; }

        public virtual string Description { get; set; }
    }
}
