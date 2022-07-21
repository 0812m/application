using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PH.Wechat.OfficialAccounts.Message.From
{
    [XmlRoot("xml")]
    [Serializable]
    public class MessageFormatBase
    {
        /// <summary>
        /// 开发者微信号
        /// </summary>
        [XmlElement("FromUserName")]
        public virtual string FromUserName { get; set; }

        /// <summary>
        /// 发送方帐号（一个OpenID）
        /// </summary>
        [XmlElement("ToUserName")]
        public virtual string ToUserName { get; set; }

        /// <summary>
        /// 消息创建时间 （整型）
        /// </summary>
        [XmlElement("CreateTime")]
        public virtual long CreateTime { get; set; }

        /// <summary>
        /// MessageId，仅在消息为普通消息时有效
        /// </summary>
        [XmlElement("MsgId")]
        public virtual int MessageId { get; set; }

        /// <summary>
        /// 事件，仅在消息为Event时有效
        /// </summary>
        [XmlElement("Event")]
        public virtual string Event { get; set; }
    }
}
