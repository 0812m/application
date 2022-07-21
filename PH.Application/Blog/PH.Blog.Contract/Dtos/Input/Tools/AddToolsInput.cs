using PH.Web.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.Dtos
{
    public class AddToolsInput:Input
    {
        public virtual string Name { get; set; }

        public virtual string Image { get; set; }

        public virtual string Description { get; set; }

        public virtual string DownloadUrl { get; set; }

        public virtual bool CanDownload { get; set; }
    }
}
