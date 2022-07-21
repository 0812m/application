using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PH.Wechat.OfficialAccounts.Message.From
{
    /// <summary>
    /// 关注/取消关注事件模型
    /// </summary>
    [XmlRoot("xml")]
    public class SubscribeEvent: MessageFormatBase
    {
        public virtual SubscribeType Subscribe 
        {
            get 
            {
                if(string.IsNullOrWhiteSpace(Event))
                    throw new ArgumentNullException(nameof(Event));
                return (SubscribeType)Enum.Parse(typeof(SubscribeType),Event,true);
            }
        }

        public enum SubscribeType 
        {
            /// <summary>
            /// 关注
            /// </summary>
            Subscribe,
            /// <summary>
            /// 取消关注
            /// </summary>
            Unsubscribe
        }
    }
}
