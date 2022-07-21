using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Web.Core.Contracts
{
    public static class ContractsExtension
    {
        /// <summary>
        /// 生成异常响应
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static Response ExceptionConvertResponse(this Exception exception) 
        {
            int errorCode = -1;
            exception = FindFriendlyException(exception);
            string message = exception.Message;
            if (exception is FriendlyException)
                errorCode = (exception as FriendlyException).Code;

            return new Response()
            {
                Code = errorCode,
                Message = message
            };  
        }

        /// <summary>
        /// 递归查找友好异常
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private static Exception FindFriendlyException(Exception exception)
        {
            if (exception is FriendlyException || exception.InnerException is null)
                return exception;
            else return FindFriendlyException(exception.InnerException);
        }
    }
}
