using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.DatabaseAccessor
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AppDbContextAttribute : Attribute
    {
        /// <summary>
        /// 数据库提供商
        /// </summary>
        public string DbProvide { get; }

        /// <summary>
        /// 连接字符串 包含 ‘=’则认为是真实的连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 是否动态上下文
        /// </summary>
        public  bool IsDynamic { get; set; }

        public AppDbContextAttribute(string dbProvide, string connectionString,bool isDynamic = false)
        {
            DbProvide = dbProvide;
            ConnectionString = connectionString;
            IsDynamic = isDynamic;
        }
    }
}
