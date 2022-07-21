using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.DatabaseAccessor.UnitOfWork
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class EnableTransactionAttribute : Attribute
    {
        public bool EnableTransaction { get; set; }
        public EnableTransactionAttribute(bool enableTransaction = true)
        {
            this.EnableTransaction = enableTransaction;
        }
    }
}
