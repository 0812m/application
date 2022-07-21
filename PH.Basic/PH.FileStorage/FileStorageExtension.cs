using Microsoft.Extensions.DependencyInjection;
using PH.FileStorage.Ftp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.FileStorage
{
    public static class FileStorageExtension
    {
        public static IServiceCollection AddAliyunService(this IServiceCollection services)
        {
            services.AddTransient<IFileStorage, FtpStorage>();
            return services;
        }
    }
}
