using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newtonsoft.Json
{
    public static class JSON
    {
        public static T Deserialize<T>(this string json,JsonSerializerSettings jsonSerializer = null) 
        {
           return JsonConvert.DeserializeObject<T>(json, jsonSerializer);
        }

        public static string Serialize<T>(this T obj, JsonSerializerSettings jsonSerializer = null) 
        {
            return JsonConvert.SerializeObject(obj, jsonSerializer);
        }
    }
}
