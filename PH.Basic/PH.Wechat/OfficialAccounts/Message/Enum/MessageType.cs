using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace PH.Wechat.OfficialAccounts.Message
{
    /// <summary>
    /// 普通消息类型
    /// </summary>
    internal enum MessageType
    {
        [Description("文本消息")]
        Text,
        [Description("图片消息")]
        Image,
        [Description("语音消息")]
        Voice,
        [Description("视频消息")]
        Video,
        [Description("小视频消息")]
        ShortVideo,
        [Description("地理位置消息")]
        Location,
        [Description("链接消息")]
        Link
    }
}
