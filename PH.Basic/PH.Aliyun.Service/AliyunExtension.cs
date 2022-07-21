using Microsoft.Extensions.DependencyInjection;
using PH.FileStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Aliyun.Service
{
    public static class AliyunExtension
    {
        /// <summary>
        /// 注入阿里云相关服务（OSS、SMS等）
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAliyunService(this IServiceCollection services) 
        {
            services.AddTransient<IFileStorage,OSSStorage>();
            return services;
        }
    }
}
