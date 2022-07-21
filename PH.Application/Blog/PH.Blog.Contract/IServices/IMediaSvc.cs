using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.IServices
{
    public interface IMediaSvc
    {
        /// <summary>
        /// 分片文件临时储存文件夹
        /// </summary>
        string TempFolder { get; set; }

        /// <summary>
        /// 文件合并
        /// </summary>
        /// <param name="hash">文件哈希值</param>
        /// <param name="targetFileName">文件名</param>
        /// <param name="fileSize">文件尺寸</param>
        /// <param name="totalPieces">分片总数</param>
        /// <returns></returns>
        Task<string> FileChunkMergeAsync(string hash, string targetFileName,double fileSize,int totalPieces);

        /// <summary>
        /// 获取分片文件最大索引
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        int GetFilePartialIndex(string hash);
    }
}
