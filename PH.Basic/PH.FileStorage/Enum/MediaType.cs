using PH.ToolsLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.FileStorage.Enum
{
    public enum MediaType
    {
        [Description("图片"), Value(new string[] { "BMP", "JPG", "JPEG", "PNG", "GIF" })]
        Image,
        [Description("视频"), Value(new string[] { "AVI", "MOV", "RMVB", "RM", "FLV", "MP4", "3GP" })]
        Video,
        [Description("文档文件"), Value(new string[] { "HTML", "PDF", "TXT", "DOC", "DOCX", "XLS", "XLSX", "PPT", "PPTX" })]
        Document
    }
}
