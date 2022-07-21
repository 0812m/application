using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.ToolsLibrary.Extension
{
    public static class TimeExtension
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp()
        {
            //DateTime.Now获取的是电脑上的当前时间
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);//精确到秒
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetUtcNowTimeStamp()
        {
            //DateTime.UtcNow获取的是世界标准时区的当前时间（比北京时间少8小时）
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds);//精确到毫秒
        }

        /// <summary>
        /// 时间戳转换为DataTime
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime TimestampToDataTime(long unixTimeStamp)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当			  地时区
            DateTime dt = startTime.AddSeconds(unixTimeStamp);
            System.Console.WriteLine(dt.ToString("yyyy/MM/dd HH:mm:ss:ffff"));
            return dt;
        }

        /// <summary>
        /// DataTime转时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long DataTimeToTimestamp(DateTime dateTime)
        {
            //new System.DateTime(1970, 1, 1)
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(dateTime); // 当地时区
            long timeStamp = (long)(DateTime.Now - startTime).TotalSeconds; // 相差秒数
            System.Console.WriteLine(timeStamp);
            return timeStamp;
        }
    }
}
