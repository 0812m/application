using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Web.Core.Cache
{
    public class MemoryCacheProvide : ICacheProvide
    {
        private readonly IDistributedCache _cache;

        public MemoryCacheProvide(IDistributedCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="relativeExpiration">过期时间（秒），若不设置则永不过期</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task SaveAsync<T>(string key,T value,int? relativeExpiration = null) 
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

           await _cache.SetStringAsync(key,Newtonsoft.Json.JsonConvert.SerializeObject(value), GetDistributedCacheEntryOptions(relativeExpiration:relativeExpiration));
        }

        /// <summary>
        /// 获取缓存过期配置
        /// </summary>
        /// <param name="absExpiration">绝对过期</param>
        /// <param name="relativeExpiration">相对过期</param>
        /// <param name="slidExpiration">滑动过期</param>
        /// <returns></returns>
        public DistributedCacheEntryOptions GetDistributedCacheEntryOptions(int? absExpiration = null,int? relativeExpiration = null,int? slidExpiration = null) 
        {
            DateTimeOffset? absoluteExpiration = null;
            if (absExpiration.HasValue)
                absoluteExpiration = DateTimeOffset.Now.AddSeconds(absExpiration.Value);

            TimeSpan? absoluteExpirationRelativeToNow = null;
            if (relativeExpiration.HasValue)
                absoluteExpirationRelativeToNow = TimeSpan.FromSeconds(relativeExpiration.Value);

            TimeSpan? slidingExpiration = null;
            if(slidExpiration.HasValue)
                slidingExpiration = TimeSpan.FromSeconds(slidExpiration.Value);

            return new DistributedCacheEntryOptions()
            {
                 AbsoluteExpiration = absoluteExpiration,
                 AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow,
                 SlidingExpiration= slidingExpiration,
            };
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task RemoveAsync(string key) 
        {
           await _cache.RemoveAsync(key);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<T?> GetAsync<T>(string key)
        {
           var val = await _cache.GetStringAsync(key);
            if(string.IsNullOrWhiteSpace(val))
                return default;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(val) ?? default;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="relativeExpiration"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key, Func<T> func,int relativeExpiration)
        {
            T? result = await GetAsync<T>(key);
            if (result == null)
            {
              result =  func.Invoke();
               await SaveAsync<T>(key,result,relativeExpiration);
            }
            return result;
        }
    }
}
