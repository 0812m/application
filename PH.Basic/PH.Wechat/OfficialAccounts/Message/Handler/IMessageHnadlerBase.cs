using PH.Wechat.OfficialAccounts.Message.From;
using PH.Wechat.OfficialAccounts.Message.To;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Wechat.OfficialAccounts.Message.Handler
{
    public interface IMessageHnadlerBase
    {
        Task<ToMessageBase> ExecuteAsync(MessageFormatBase messageFormat);
    }
}
