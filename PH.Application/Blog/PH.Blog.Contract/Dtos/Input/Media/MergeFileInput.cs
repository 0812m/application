using PH.Web.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.Dtos
{
    /// <summary>
    /// 文件合并 Input
    /// </summary>
    public class MergeFileInput:Input
    {
        /// <summary>
        /// 文件MD5值
        /// </summary>
        public virtual string Hash { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public virtual string FileName { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public virtual double FileSize { get; set; }

        /// <summary>
        /// 总分片数
        /// </summary>
        public virtual int TotalPieces { get; set; }
    }
}
