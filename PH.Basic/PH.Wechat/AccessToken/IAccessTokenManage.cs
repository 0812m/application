using PH.Wechat.ResponseMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Wechat
{
    public interface IAccessTokenManage
    {
        /// <summary>
        /// 刷新 access token
        /// </summary>
        /// <returns></returns>
        Task<AccessToken> RefreshAsync();

        /// <summary>
        /// 获取 access token
        /// </summary>
        /// <returns></returns>
        Task<string> GetAsync();

        Task SetAsync(string token);
    }
}
