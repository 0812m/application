using Microsoft.AspNetCore.Mvc;
using PH.Web.Core.Mvc.DynamicWebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class  DynamicWebApiExtension
    {
        public static IMvcBuilder AddDynamicWebApi(this IMvcBuilder mvcBuilder) 
        {
            if (mvcBuilder == null)
            {
                throw new ArgumentNullException(nameof(mvcBuilder));
            }

            mvcBuilder.ConfigureApplicationPartManager(applicationPartManager =>
            {
                applicationPartManager.FeatureProviders.Add(new DynamicApiControllerFeatureProvider());
            });

            mvcBuilder.Services.Configure<MvcOptions>(options =>
            {
                options.Conventions.Add(new DynamicApiApplicationServiceConvention());
            });

            return mvcBuilder;
        }

        public static IMvcCoreBuilder AddDynamicWebApi(this IMvcCoreBuilder mvcBuilder)
        {
            if (mvcBuilder == null)
            {
                throw new ArgumentNullException(nameof(mvcBuilder));
            }

            mvcBuilder.ConfigureApplicationPartManager(applicationPartManager =>
            {
                applicationPartManager.FeatureProviders.Add(new DynamicApiControllerFeatureProvider());
            });

            mvcBuilder.Services.Configure<MvcOptions>(options =>
            {
                options.Conventions.Add(new DynamicApiApplicationServiceConvention());
            });

            return mvcBuilder;
        }
    }
}
