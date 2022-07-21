using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Web.Core.Cache
{
    /// <summary>
    /// 缓存提供器
    /// </summary>
    public interface ICacheProvide
    {
        /// <summary>
        /// 根据 key 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="relativeExpiration">相对过期时间 单位：秒</param>
        /// <returns></returns>
        Task SaveAsync<T>(string key, T value, int? relativeExpiration = null);

        Task RemoveAsync(string key);

        Task<T?> GetAsync<T>(string key);

        /// <summary>
        ///  根据 key 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存key</param>
        /// <param name="func">数据提供</param>
        /// <param name="relativeExpiration">相对过期时间 单位：秒</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key,  Func<T> func, int relativeExpiration);
    }
}
