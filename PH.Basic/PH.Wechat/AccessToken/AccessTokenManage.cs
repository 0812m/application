using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.ToolsLibrary.Http;
using PH.Wechat.ResponseMessage;

namespace PH.Wechat
{
    /// <summary>
    /// token管理类
    /// </summary>
    public class AccessTokenManage : IAccessTokenManage
    {
        public readonly WechatOptions wechatOptions;
        public AccessTokenManage(IOptions<WechatOptions> options)
        {
            wechatOptions = options.Value;
        }

        private static string accessToken; 

        public async Task<AccessToken> RefreshAsync()
        {
            var result = await RequestBuilder.DefaultBuilder()
                .SetUrl($"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={wechatOptions.AppID}&secret={wechatOptions.Appsecret}")
                .UseMethod(HttpMethod.Get)
                .SendAsync<AccessToken>() ;
            SetAsync(result?.access_token);
            return result;
        }

        public async Task<string> GetAsync()
        {
            return await Task.FromResult(accessToken);
        }

        public Task SetAsync(string token)
        {
            accessToken = token;
            return Task.CompletedTask;
        }
    }
}
