using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PH.Blog.Contract;
using PH.Blog.Contract.Dtos;
using PH.Blog.Contract.IApis;
using PH.Blog.Contract.IServices;
using PH.Web.Core;
using PH.Web.Core.Authentication;
using PH.Web.Core.Cache;
using PH.Web.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Application.Apis
{
    [AuthorizeApp, Route("api/[controller]"), ApiDescriptionSettings("媒体数据模块")]
    public class MediaApis : BaseApiController, IMediaApis
    {
        private readonly IMediaSvc _mediaSvc;

        public MediaApis(IMediaSvc mediaSvc, IConfigurtionSvc configurtionSvc, IHttpContextAccessor httpContextAccessor, ICacheProvide cache, Identity identity)
            : base(configurtionSvc, httpContextAccessor, cache)
        {
            _mediaSvc = mediaSvc;
        }

        /// <summary>
        /// 分片上传
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [Route("upload/part"), SecurityDefinition(SecurityConst.MEDIA_UPLOAD_PART)]
        public async Task PartialUploadAsync()
        {
            var index = -1;//分片索引
            var hash = string.Empty;
            var from = HttpContext.Request.Form;

            if (!MediaTypeHeaderValue.TryParse(HttpContext.Request.Headers.ContentType.ToString(), out var mediaTypeHeader))
                throw Sorry.Bad("提交数据异常");

            if (!from.Files.Any())
                throw Sorry.Bad("未上传文件");

            if (!from.ContainsKey("hash"))
                throw Sorry.Bad("文件hash值缺失");
            else
                hash = from["hash"].ToString();

            if (!from.ContainsKey("index"))
                throw Sorry.Bad("分片索引缺失");
            else
                int.TryParse(from["index"].ToString(), out index);

            if (index == 0)
            {
                //第一块文件块，从数据库中检查是否已上传过
                //如果已经上传过直接返回，否则往数据库添加一条数据
            }

            var file = from.Files.FirstOrDefault();
            var tempFolder = Path.Combine(_mediaSvc.TempFolder, hash);
            if (!Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);
            using (FileStream fs = new FileStream(Path.Combine(tempFolder, index.ToString()), FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }
        }

        /// <summary>
        /// 合并文件
        /// </summary>
        /// <returns></returns>
        [Route("merge"), SecurityDefinition(SecurityConst.MEDIA_MERGE)]
        public async Task<string> MergeFileAsync(MergeFileInput input)
        {
            var mergeFilePath = await _mediaSvc.FileChunkMergeAsync(input.Hash, input.FileName, input.FileSize, input.TotalPieces);
            return mergeFilePath;
        }

        /// <summary>
        /// 获取已上传文件的分片index
        /// </summary>
        /// <returns></returns>
        [Route("part/index"), SecurityDefinition(SecurityConst.MEDIA_PART_INDEX)]
        public async Task<int> GetFilePartialIndexAsync([FromQuery] GetFileIndexInput input)
        {
            return await Task.FromResult(_mediaSvc.GetFilePartialIndex(input.Hash));
        }
    }
}
