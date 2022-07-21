using PH.Wechat.ResponseMessage;
using PH.Wechat.ResponseMessage.Input;
using PH.Wechat;
using PH.ToolsLibrary.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Wechat.OfficialAccounts
{
    public class WxAccountService : IWxAccountService
    {
        private readonly IAccessTokenManage accessTokenManage;

        public WxAccountService(IAccessTokenManage accessTokenManage)
        {
            this.accessTokenManage = accessTokenManage;
        }
        public async Task<Ticket> GenerateQRcode(TicketInput ticketInput)
        {
           return await RequestBuilder.DefaultBuilder()
                .SetUrl($"https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={await accessTokenManage.GetAsync()}")
                .UseMethod(HttpMethod.Post)
                .SetBody(ticketInput)
                .SendAsync<Ticket>();
        }
    }
}
