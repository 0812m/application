using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using PH.ToolsLibrary.Extension;
using PH.Wechat.OfficialAccounts.Message.From;

namespace PH.Wechat.OfficialAccounts.Message
{
    public class MessagePool : IMessagePool
    {
         private  static object _lock = new object();
         private static List<MessageFormatBase> _pool = new List<MessageFormatBase>(50);

        public bool TryAdd(MessageFormatBase message,bool isEvent)
        {
            var canAdd = false;
            if (isEvent)
            {
                canAdd = !_pool.Any(x => x.FromUserName.Equals(message.FromUserName) && x.CreateTime == message.CreateTime);
            }
            else
            {
                canAdd = !_pool.Any(x => x.MessageId == message.MessageId);
            }

            if (canAdd)
            {
                lock (_lock) 
                {
                    if (_pool.Count + 1 >= _pool.Capacity)
                        _pool.RemoveAll(x => TimeExtension.GetTimeStamp() - x.CreateTime > 20);
                    _pool.Add(message);
                }
            }
            return canAdd;
        }
    }
}
