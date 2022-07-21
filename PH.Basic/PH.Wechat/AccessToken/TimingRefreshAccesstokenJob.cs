using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PH.Wechat.ResponseMessage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Wechat
{
    internal class TimingRefreshAccesstokenJob : BackgroundService
    {
        public WechatOptions WechatOptions { get; }
        public ILogger<TimingRefreshAccesstokenJob> Logger { get; }
        public IAccessTokenManage AccessToken { get; }

        public TimingRefreshAccesstokenJob
            (IOptions<WechatOptions> options,ILogger<TimingRefreshAccesstokenJob> logger,IAccessTokenManage accessToken)
        {
            WechatOptions = options.Value;
            Logger = logger;
            AccessToken = accessToken;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Logger.LogInformation($"开始执行，执行间隔：{WechatOptions.RefreshTokenIntervalTime}分钟");
                Logger.LogInformation($"开始刷新token");
                var result = await AccessToken.RefreshAsync();
                Logger.LogInformation($"token已经刷新，{result.Serialize()}");
                await Task.Delay(1000 * 60 * WechatOptions.RefreshTokenIntervalTime, stoppingToken);
            }
        }
    }
}
