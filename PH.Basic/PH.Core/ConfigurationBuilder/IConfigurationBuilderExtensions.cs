using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Core.ConfigurationBuilder
{
    public static class IConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder LoadJsonConfiguration(this IConfigurationBuilder builder)
        {
            //添加配置源
            var baseDir = AppContext.BaseDirectory;
            var types = ApplicationContext.Assemblies.SelectMany(x => x.GetTypes());
            var currentDir = Directory.GetCurrentDirectory();
            var baseDirJsonFilePaths = Directory.GetFiles(baseDir, "*.json", SearchOption.TopDirectoryOnly);
            var currentDirJsonFilePaths = Directory.GetFiles(currentDir, "*.json", SearchOption.TopDirectoryOnly);
            var jsonFilePaths = baseDirJsonFilePaths.Union(currentDirJsonFilePaths).Where(x => !ApplicationContext.runtimeJsonSuffixs.Any(a => x.EndsWith(a)));

            foreach (var path in jsonFilePaths)
            {
                builder.AddJsonFile(path, false, true);
            }

            return builder;
        }
    }
}
