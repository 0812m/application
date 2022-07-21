using PH.Blog.Contract.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Core.IOC.Attributes;
using PH.Core;
using PH.Web.Core.Contracts;
using PH.Blog.Contract;

namespace PH.Blog.Application.ServiceImpl
{
    [AutoInjection]
    public class MediaSvc : IMediaSvc
    {
        private string _staticFolder { get => Path.Combine(ApplicationContext.WebHostingEnvironment.WebRootPath, "Static"); }
        private string _tempFolder { get => Path.Combine(_staticFolder, "Temp"); }
        string IMediaSvc.TempFolder
        {
            get => _tempFolder;
            set => throw new NotImplementedException();
        }

        /// <summary>
        /// 文件合并
        /// </summary>
        /// <param name="hash">文件hash值</param>
        /// <param name="targetFileName">合并文件名</param>
        /// <param name="fileSize">文件大小</param>
        /// <param name="totalPieces">分片总数</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<string> FileChunkMergeAsync(string hash, string targetFileName, double fileSize, int totalPieces)
        {
            var folder = Path.Combine(_tempFolder,hash);
            if (!Directory.Exists(folder))
                throw Sorry.Bad(ErrorCodes.FileNotExist);

           var files = Directory.GetFiles(folder).OrderBy(x => int.Parse(Path.GetFileName(x))).ToList();
            if (files.Count != totalPieces)
                throw Sorry.Bad(ErrorCodes.FilePartialAbnormal);

            var mergeFileSavePath = Path.Combine(_staticFolder, targetFileName);
            using (FileStream fs = new FileStream(mergeFileSavePath,FileMode.Create))
            {
                foreach (var fileChunk in files)
                {
                   var buffer = await File.ReadAllBytesAsync(fileChunk);
                   await fs.WriteAsync(buffer);
                   File.Delete(fileChunk);
                }
            }

            return mergeFileSavePath;
        }

        /// <summary>
        /// 获取文件分片最大index
        /// </summary>
        /// <returns></returns>
        public int GetFilePartialIndex(string hash) 
        {
            var folder = Path.Combine(_tempFolder, hash);
            if (!Directory.Exists(folder))
                throw Sorry.Bad(ErrorCodes.FileNotExist);
            var max = Directory.GetFiles(folder).MaxBy(x => int.Parse(Path.GetFileName(x)));
            return int.Parse(max);
        }
    }
}
