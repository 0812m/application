using PH.ToolsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PH.FileStorage.Ftp
{
    public class FtpStorage : IFileStorage
    {
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string filePath)
        {
            var req = CreateRequest(filePath, WebRequestMethods.Ftp.DeleteFile);
            await req.GetResponseAsync();
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="targetFilePath"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task DownloadAsync(string targetFilePath, string savePath)
        {
            if (string.IsNullOrWhiteSpace(savePath))
                throw new ArgumentNullException("储存本地路径不能为空");

            //获取绝对路径
            var absPath = Path.GetFullPath(savePath);
            //获取最后一层目录
            var dir = savePath[..absPath.LastIndexOf(Path.DirectorySeparatorChar)];
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var req = CreateRequest(targetFilePath, WebRequestMethods.Ftp.DownloadFile);
            try
            {
                var resp = await req.GetResponseAsync();
                using (FileStream fileStream = new FileStream(absPath, FileMode.Create))
                using (var starem = resp.GetResponseStream())
                {
                    await starem.CopyToAsync(fileStream);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="oldFileName"></param>
        /// <param name="newFileName"></param>
        /// <returns></returns>
        public async Task RenameAsync(string oldFileName, string newFileName)
        {
            var req = CreateRequest(oldFileName, WebRequestMethods.Ftp.Rename);
            req.RenameTo = newFileName;
            await req.GetResponseAsync();
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public async Task UploadAsync(Stream stream, string savePath)
        {
            try
            {
                await CreateDirectoryAsync(savePath);
                var req = CreateRequest(savePath, WebRequestMethods.Ftp.UploadFile);
                using (var s = req.GetRequestStream())
                {
                    await stream.CopyToAsync(s);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 获取文件夹或文件信息
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public async Task<string> GetDirectoryOrFileInfo(string dirPath)
        {
            var request = CreateRequest(dirPath, WebRequestMethods.Ftp.ListDirectoryDetails);
            var resp = await request.GetResponseAsync();
            return await resp.AsStringAsync();

        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="isCreate"></param>
        /// <returns></returns>
        public async Task CreateDirectoryAsync(string dir, bool isCreate = false)
        {
            if (isCreate)
            {
                //550 Create directory operation failed.
                var req = CreateRequest(dir, WebRequestMethods.Ftp.MakeDirectory);
                try
                {
                    var resp = await req.GetResponseAsync();
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                var realdir = dir;
                if (Path.HasExtension(dir))
                    realdir = dir[..dir.LastIndexOf('/')];
                await CreateDirectoryAsync(realdir.Split('/'));
            }
        }
        public async Task CreateDirectoryAsync(string[] dir)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in dir)
            {
                sb.Append($"/{item}");
                var realPath = sb.ToString();
                var dirStr = await GetDirectoryOrFileInfo(realPath);
                if (string.IsNullOrWhiteSpace(dirStr))
                    await CreateDirectoryAsync(realPath, true);
            }
        }

        private FtpWebRequest CreateRequest(string savePath, string method)
        {
            return CreateRequest(new FtpOptions() { SavePath = savePath }, method);
        }

        private FtpWebRequest CreateRequest(FtpOptions options, string method)
        {
            Uri uri = new Uri(options.Url);
            var req = (FtpWebRequest)WebRequest.Create(uri);
            req.Method = method;
            req.Credentials = new NetworkCredential(options.User, options.Password);
            req.EnableSsl = options.EnableSsl;
            req.KeepAlive = options.KeepAlive;
            req.UseBinary = options.UseBinary;
            return req;
        }
    }
}
