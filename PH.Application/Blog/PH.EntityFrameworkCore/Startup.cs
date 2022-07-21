using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PH.Core;
using PH.Core.Application.Attributes;

namespace PH.EntityFrameworkCore
{
    [Startup(300)]
    public class Startup:StartupModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddDatabaseAccessor(s => 
            {
                s.AddDbContextToPool<DefaultDbContext>();
            });
        }
    }
}
