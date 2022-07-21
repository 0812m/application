using PH.Wechat.ResponseMessage;
using PH.Wechat.ResponseMessage.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Wechat.OfficialAccounts
{
    /// <summary>
    /// 账号管理
    /// </summary>
    public interface IWxAccountService
    {
        /// <summary>
        /// 生成带参数的二维码
        /// </summary>
        /// <param name="ticketInput"></param>
        /// <returns></returns>
        Task<Ticket> GenerateQRcode(TicketInput ticketInput);
    }
}
