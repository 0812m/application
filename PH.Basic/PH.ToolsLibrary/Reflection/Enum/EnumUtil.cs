using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace PH.ToolsLibrary.Reflection
{
    public static class EnumUtil
    {
        /// <summary>
        /// 获取指定特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="inherit">是否向上检索</param>
        /// <returns></returns>
        public static T GetAttribute<T>(this object obj,bool inherit = false) 
            where T : Attribute
        {
           var type = obj.GetType();
            T result = default;
            if (type.IsEnum)
            {
               result = type.GetFields().FirstOrDefault(x => x.Name.Equals(obj.ToString()))?.GetCustomAttribute<T>();
            }
            else
            {
               result = type.GetCustomAttribute<T>();
            }
            return result;
        }
    }
}
