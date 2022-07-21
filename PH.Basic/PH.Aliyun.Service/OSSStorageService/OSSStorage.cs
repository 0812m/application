using PH.ToolsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Aliyun.OSS.Model;
using Aliyun.OSS;
using System.Security.AccessControl;
using PH.FileStorage;

namespace PH.Aliyun.Service
{
    /// <summary>
    /// 详细查阅：https://help.aliyun.com/document_detail/410750.html
    /// </summary>
    public class OSSStorage : IFileStorage
    {
        private OSSOptions _options;
        private OssClient _ossClient;

        public OSSStorage(string endpoint = null, string bucket = null)
            : this(new OSSOptions(endpoint: endpoint, bucket: bucket))
        {

        }

        public OSSStorage(OSSOptions options)
        {
            _options = options;
            _ossClient = new OssClient(_options.Endpoint, _options.AccessKey, _options.Secret);
        }

        public async Task DeleteAsync(string filePath)
        {
            try
            {
                var key =  GetKeyByPath(filePath);
                if(string.IsNullOrWhiteSpace(key))
                    throw new FileNotFoundException($"{filePath}文件不存在");
                // 删除文件。
                await Task.Run(() => _ossClient.DeleteObject(_options.Bucket, key));
                Console.WriteLine("Delete object succeeded");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete object failed. {0}", ex.Message);
            }
        }

        public async Task DownloadAsync(string targetFilePath, string savePath)
        {
            try
            {
                // 下载文件到流。OssObject 包含了文件的各种信息，如文件所在的存储空间、文件名、元信息以及一个输入流。
                var key = GetKeyByPath(targetFilePath);
                var obj = _ossClient.GetObject(_options.Bucket, key);
                await Task.Run(() =>
                 {
                     using (var requestStream = obj.Content)
                     {
                         byte[] buf = new byte[1024];
                         var fs = File.Open(savePath, FileMode.OpenOrCreate);
                         var len = 0;
                        // 通过输入流将文件的内容读取到文件或者内存中。
                        while ((len = requestStream.Read(buf, 0, 1024)) > 0)
                         {
                             fs.Write(buf, 0, len);
                         }
                         fs.Close();
                     }
                 });
                Console.WriteLine("Get object succeeded");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get object failed. {0}", ex.Message);
            }
        }

        public Task RenameAsync(string oldFileName, string newFileName)
        {
            throw new NotImplementedException();
        }

        public async Task UploadAsync(Stream stream, string savePath)
        {
            try
            {
                var obj = await Task.Run(() => _ossClient.PutObject(_options.Bucket, savePath, stream));
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 创建存储空间
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public async Task<Bucket> CreateBucket(string bucketName)
        {
            Bucket bucket = default;
            try
            {
                var request = new CreateBucketRequest(bucketName);
                //设置读写权限ACL为公共读PublicRead，默认为私有权限。
                request.ACL = CannedAccessControlList.PublicRead;
                //设置数据容灾类型为同城冗余存储。
                request.DataRedundancyType = DataRedundancyType.ZRS;
                bucket = await Task.Run(() => _ossClient.CreateBucket(request));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create bucket failed. {0}", ex.Message);
            }
            return bucket;
        }

        /// <summary>
        /// 判断存储空间是否存在
        /// </summary>
        /// <param name="bucketName"></param>
        public async Task<bool> DoesBucketExist(string bucketName)
        {
            bool exist = false;
            try
            {
                exist = await Task.Run(() => _ossClient.DoesBucketExist(bucketName));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Check object Exist failed. {0}", ex.Message);
            }
            return exist;
        }

        /// <summary>
        /// 返回指定路径字符串的文件名，不带扩展名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetKeyByPath(string path) 
        {
           return Path.GetFileNameWithoutExtension(path);
        }
    }
}
