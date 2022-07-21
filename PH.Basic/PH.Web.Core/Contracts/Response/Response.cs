using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Web.Core.Contracts
{
    [Serializable]
    public class Response<T>
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }

        public long Timestamp => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        public Response() : this(default)
        {

        }

        public Response(T data)
        {
            Code = 200;
            Message = "Success";
            Data = data;
        }
    }

    [Serializable]
    public class Response : Response<object>
    {
        public Response():this(null)
        {

        }

        public Response(object obj):base(obj)
        {

        }
    }
}
