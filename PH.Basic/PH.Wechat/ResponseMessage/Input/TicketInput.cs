using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Wechat.ResponseMessage.Input
{
    /// <summary>
    /// 获取 Ticket 参数模型
    /// </summary>
    public class TicketInput
    {
        /// <summary>
        /// 二维码有效时间，以秒为单位。 最大不超过2592000（即30天），此字段如果不填，则默认有效期为60秒。
        /// </summary>
        public int? expire_seconds { get; set; }

        /// <summary>
        /// 二维码类型
        /// <remark>QR_SCENE为临时的整型参数值</remark>
        /// <remark>QR_STR_SCENE为临时的字符串参数值</remark>
        /// <remark>QR_LIMIT_SCENE为永久的整型参数值</remark>
        /// <remark>QR_LIMIT_STR_SCENE为永久的字符串参数值</remark>
        /// </summary>
        public string action_name { get; set; }

        /// <summary>
        ///二维码详细信息
        /// </summary>
        public object action_info { get; set; }
    }
}
