using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PH.Wechat;
using PH.Wechat.OfficialAccounts;
using PH.Wechat.OfficialAccounts.Message;
using PH.Wechat.OfficialAccounts.Message.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WechatExtension
    {
        /// <summary>
        /// 添加微信服务库
        /// </summary>
        /// <param name="services"></param>
        public static void AddWechatService(this IServiceCollection services) 
        {
            var factory = services.FirstOrDefault(x => x.ServiceType == typeof(IConfiguration))?.ImplementationFactory;
            var IConfiguration = factory?.Invoke(null) as IConfiguration;
            services.Configure<WechatOptions>(IConfiguration.GetSection("Wechat"));

            services.AddScoped<IWxMessageHandle, WxMessageHandle>();
            services.AddSingleton<IMessagePool,MessagePool>();
            services.AddTransient<IAccessTokenManage, AccessTokenManage>();
            services.AddSingleton<IHostedService, TimingRefreshAccesstokenJob>();
            services.AddHostedService<TimingRefreshAccesstokenJob>();
            services.AddScoped<IWxAccountService, WxAccountService>();
        }

        public static void UseWechatService(this IApplicationBuilder  app) 
        {

        }
    }
}
