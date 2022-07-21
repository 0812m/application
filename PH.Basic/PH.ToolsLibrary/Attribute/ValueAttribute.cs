using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.ToolsLibrary
{
    public class ValueAttribute: Attribute
    {
        public object Value { get; set; }

        public ValueAttribute(object value)
        {
            Value = value;
        }
    }
}
