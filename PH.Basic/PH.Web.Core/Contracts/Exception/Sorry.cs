using PH.ToolsLibrary.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Web.Core.Contracts
{
    public  class Sorry
    {
        /// <summary>
        /// 卧槽，报错了
        /// </summary>
        /// <returns></returns>
        public static FriendlyException Wocao(string message,int code = 10086) 
        {
            return new FriendlyException(message,code);
        }

        /// <summary>
        /// 卧槽，报错了
        /// </summary>
        /// <returns></returns>
        public static FriendlyException Wocao(object @enum, params object[] objs)
        {
            var messageFormat = @enum.GetAttribute<ErrorMetaDataAttribute>()?.MessageFormat;
            if (objs is not null)
                messageFormat = string.Format(messageFormat, objs);

            return new FriendlyException(messageFormat, (int)@enum);
        }

        /// <summary>
        /// 规范抛错
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static FriendlyException Bad(string message, int code = 10086)
        {
            return new FriendlyException(message, code);
        }

        /// <summary>
        ///  规范抛错
        /// </summary>
        /// <param name="enum"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static FriendlyException Bad(object @enum, params object[] objs)
        {
            var messageFormat = @enum.GetAttribute<ErrorMetaDataAttribute>()?.MessageFormat;
            if (objs is not null)
                messageFormat = string.Format(messageFormat, objs);

            return new FriendlyException(messageFormat, (int)@enum);
        }
    }
}
