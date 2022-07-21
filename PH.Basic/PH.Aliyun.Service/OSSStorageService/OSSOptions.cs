using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Aliyun.Service
{
    public class OSSOptions
    {
        public virtual string AccessKey { get; set; }

        public virtual string Secret { get; set; }

        public virtual string Endpoint { get; set; }

        public virtual string Bucket { get; set; }
        public OSSOptions(string accessKey = null, string secret = null, string endpoint = null, string bucket = null)
        {
           
        }
    }
}
