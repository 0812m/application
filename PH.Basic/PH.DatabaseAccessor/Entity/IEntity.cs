using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.DatabaseAccessor.Entity
{
    internal interface IEntity<T>:IEntity, IEntityTypeConfiguration<T>
        where T : class
    {
        
    }

    /// <summary>
    /// 实体依赖接口，所有实体需实现该接口
    /// </summary>
    internal interface IEntity
    {
    }
}
