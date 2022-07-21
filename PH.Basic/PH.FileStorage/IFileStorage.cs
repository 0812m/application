namespace PH.FileStorage
{
    public interface IFileStorage
    {
        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        Task UploadAsync(Stream stream, string savePath);

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        Task DeleteAsync(string filePath);

        /// <summary>
        /// 下载
        /// </summary>
        /// <returns></returns>
        Task DownloadAsync(string targetFilePath, string savePath);

        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="oldFileName">待改名文件全路径</param>
        /// <param name="newFileName">新名字</param>
        /// <returns></returns>
        Task RenameAsync(string oldFileName, string newFileName);
    }
}