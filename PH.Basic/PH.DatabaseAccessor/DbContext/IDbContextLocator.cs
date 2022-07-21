using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.DatabaseAccessor
{
    /// <summary>
    /// 数据库定位器
    /// </summary>
    public interface IDbContextLocator
    {
    }

    public class MasterContextLocator : IDbContextLocator { }

    public class SlaveContextLocator : IDbContextLocator { }
}
