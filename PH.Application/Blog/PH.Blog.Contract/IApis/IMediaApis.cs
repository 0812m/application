using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.IApis
{
    public interface IMediaApis
    {
        /// <summary>
        /// 分片上传
        /// </summary>
        /// <returns></returns>
        Task PartialUploadAsync();
    }
}
