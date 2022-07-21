using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Web.Core.Contracts
{
    /// <summary>
    /// 友好异常
    /// </summary>
    public class FriendlyException: Exception
    {
        /// <summary>
        /// 应用场景：手动填写错误消息及错误码
        /// </summary>
        /// <param name="errMsg"></param>
        /// <param name="errCode"></param>
        public FriendlyException(string errMsg, int errCode)
        {
            Code = errCode;
            _message = errMsg;
        }

        private string _message { get; set; }
        public override string Message => _message;

        /// <summary>
        /// 错误码
        /// </summary>
        public int Code { get; set; }
    }
}
