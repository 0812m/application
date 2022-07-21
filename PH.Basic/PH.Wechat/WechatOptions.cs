namespace PH.Wechat
{
    /// <summary>
    /// 微信配置
    /// </summary>
    public class WechatOptions
    {
        public virtual string AppID { get; set; }

        public virtual string Appsecret { get; set; }

        public virtual string Token { get; set; }

        /// <summary>
        /// token 刷新间隔时间（单位：分钟）
        /// </summary>
        public virtual int RefreshTokenIntervalTime { get; set; }
    }
}