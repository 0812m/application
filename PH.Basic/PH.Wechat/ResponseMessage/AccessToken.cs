using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Wechat.ResponseMessage
{
    /// <summary>
    /// 获取Access token
    /// </summary>
    public class AccessToken
    {
        public virtual string access_token { get; set; }

        public long expires_in { get; set; }
    }
}
