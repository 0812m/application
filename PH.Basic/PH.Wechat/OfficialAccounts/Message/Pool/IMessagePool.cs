using PH.Wechat.OfficialAccounts.Message.From;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Wechat.OfficialAccounts.Message
{
    public interface IMessagePool
    {
        bool TryAdd(MessageFormatBase message, bool isEvent);
    }
}
