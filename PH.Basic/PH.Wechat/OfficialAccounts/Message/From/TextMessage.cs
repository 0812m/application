using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PH.Wechat.OfficialAccounts.Message.From
{
    /// <summary>
    /// 文本消息模型
    /// </summary>
    [XmlRoot("xml")]
    public class TextMessage: MessageFormatBase
    {
    }
}
