using Microsoft.EntityFrameworkCore.Query;
using PH.DatabaseAccessor.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore
{
    public static class EFCoreExtensions
    {
        /// <summary>
        /// [EF Core] 根据条件成立再构建 Include 查询
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <typeparam name="TProperty">泛型属性类型</typeparam>
        /// <param name="sources">集合对象</param>
        /// <param name="condition">布尔条件</param>
        /// <param name="expression">新的集合对象表达式</param>
        /// <returns></returns>
        public static IIncludableQueryable<TSource, TProperty> Include<TSource, TProperty>(this IQueryable<TSource> sources, bool condition, Expression<Func<TSource, TProperty>> expression) where TSource : class
        {
            return condition ? sources.Include(expression) : (IIncludableQueryable<TSource, TProperty>)sources;
        }

        /// <summary>
        /// [EF Core] 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">数据源</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <returns></returns>
        public static async Task<(int Total, int TotalPage, IEnumerable<T> Items)> ToPagedAsync<T>(this IQueryable<T> query, int pageIndex, int pageSize)
            where T : class
        {
            int total = await query.CountAsync();
            int totalPage = (total - 1) / pageSize + 1;
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            IEnumerable<T> results = await query.ToListAsync();
            return (total, totalPage, results);
        }

        /// <summary>
        /// [EF Core] 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query">数据源</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static async Task<(int Total, int TotalPage, IEnumerable<object> Items)> ToPagedAsync<T>(this IQueryable<T> query, int pageIndex, int pageSize, Func<T,int, object> func)
        {
            int total = await query.CountAsync();
            int totalPage = (total - 1) / pageSize + 1;
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            IEnumerable<object> results = query.Select(func).ToList();
            return (total, totalPage, results);
        }
    }
}
