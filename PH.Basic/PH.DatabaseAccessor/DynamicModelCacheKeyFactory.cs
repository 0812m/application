using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.DatabaseAccessor
{
    internal class DynamicModelCacheKeyFactory: IModelCacheKeyFactory
    {
        private static int seed = 0;

        /// <summary>
        /// 刷新模型，只要Create返回的值跟上次缓存的值不一样，EFCore就认为模型已经更新，需要重新加载
        /// </summary>
        public static void RefreshModel() 
        {
            Interlocked.Increment(ref seed);
        }

        public object Create(DbContext context) => (context.GetType(),seed);
    }
}
