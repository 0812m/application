using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Core.Application.Attributes
{
    public class StartupAttribute: Attribute
    {
        public int Order;
        public StartupAttribute(int order)
        {
            Order = order;
        }
    }
}
