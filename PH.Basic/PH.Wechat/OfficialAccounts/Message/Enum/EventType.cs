using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace PH.Wechat.OfficialAccounts.Message
{
    /// <summary>
    /// 事件类型
    /// </summary>
    internal enum EventType
    {
        [Description("关注事件")]
        Subscribe,
        [Description("取消关注事件")]
        UnSubscribe,
        [Description("扫描带参数二维码事件(已关注的用户)")]
        Scan,
        [Description("上报地理位置事件")]
        Location,
        [Description("自定义菜单事件")]
        Click
    }
}
