using Microsoft.AspNetCore.Http;
using PH.Wechat.OfficialAccounts.Message.To;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Wechat.OfficialAccounts.Message.Handler
{
    /// <summary>
    /// 公众号消息处理接口
    /// </summary>
    public interface IWxMessageHandle
    {
        Task<ToMessageBase> HandlerAsync(HttpContext httpcontext);
    }
}
