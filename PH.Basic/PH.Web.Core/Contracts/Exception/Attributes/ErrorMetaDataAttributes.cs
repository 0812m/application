using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Web.Core.Contracts
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ErrorMetaDataAttribute: Attribute
    {
        public virtual string MessageFormat { get; set; }

        public ErrorMetaDataAttribute(string messageFormat)
        {
            MessageFormat = messageFormat;
        }
    }
}
