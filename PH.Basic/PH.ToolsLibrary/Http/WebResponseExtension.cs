using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PH.ToolsLibrary
{
    public static class WebResponseExtension
    {
        public async static Task<T> AsAsync<T>(this WebResponse response)
            where T : class,new()
        {
            var result = await System.Text.Json.JsonSerializer.DeserializeAsync<T>(response.GetResponseStream(), new System.Text.Json.JsonSerializerOptions()
            {
                //忽略大小写
                PropertyNameCaseInsensitive = true,
                // Text.Json默认只对属性进行 Serializer(序列化) Deserialize(反序列化) 处理，将 IncludeFields 设为 True 则包含字段。
                IncludeFields = true
            });
            response.Close();

            return result;
        }

        public async static Task<string> AsStringAsync(this WebResponse response)
        {
            using (var readStarem = new StreamReader(response.GetResponseStream()))
            {
                return await readStarem.ReadToEndAsync();
            }
        }
    }
}
