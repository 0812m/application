using PH.DatabaseAccessor.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Entity
{
    /// <summary>
    /// 访问日志
    /// </summary>
    public class AcessLogs:BaseEntity<AcessLogs,int>
    {
        public virtual string IP { get; set; }
    }
}
