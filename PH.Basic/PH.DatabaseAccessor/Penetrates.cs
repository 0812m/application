using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.DatabaseAccessor
{
    internal static class Penetrates
    {
        /// <summary>
        /// 缓存 DbContext 的定位器
        /// </summary>
        internal static readonly ConcurrentDictionary<Type, Type> DbContextWithLocatorCached;

        /// <summary>
        /// 带有AppDbContextAttribute特性的缓存
        /// </summary>
        internal static readonly ConcurrentDictionary<AppDbContextAttribute, Type> DbContextWithAttributeCached;

        static Penetrates()
        {
            DbContextWithLocatorCached = new ConcurrentDictionary<Type, Type>();
            DbContextWithAttributeCached = new ConcurrentDictionary<AppDbContextAttribute, Type>();
        }
    }
}
