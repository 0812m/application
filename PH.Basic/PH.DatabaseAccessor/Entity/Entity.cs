using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.DatabaseAccessor.Entity
{
    public abstract class Entity<T> : IEntity<T>
         where T : class
    {
        public Entity()
        {

        }

        public void Configure(EntityTypeBuilder<T> builder)
        {
            Mapping(builder);
        }

        /// <summary>
        /// 仅仅是不想用 Configure 方法名罢了
        /// </summary>
        /// <param name="entityTypeBuilder"></param>
        protected virtual void Mapping(EntityTypeBuilder<T> entityTypeBuilder) { }
    }


    /// <summary>
    /// 自带 Id 主键实体
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class Entity<T, TKey> : Entity<T> where T : class
    {
        [System.ComponentModel.DataAnnotations.Key]
        public virtual TKey Id { get; set; }
    }

    /// <summary>
    /// 自带 Id 主键 与 CreateTime 字段实体
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class BaseEntity<T, TKey> : Entity<T, TKey> where T : class
    {
        public virtual DateTime CreateTime { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 自带 Id 主键 且 用于逻辑删除实体
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class CanDelEntity<T, TKey> : BaseEntity<T, TKey> where T : class
    {
        public virtual bool IsDeleted { get; set; }
    }
}
