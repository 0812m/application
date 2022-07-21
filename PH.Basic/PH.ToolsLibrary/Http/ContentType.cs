using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.ToolsLibrary.Http
{
   public enum ContentType
    {
        [Value("text/html"),]
        Html,
        [Value("text/plain")]
        Plain,
        [Value("text/xml")]
        Xml,
        [Value("application/octet-stream")]
        Stream,
        [Value("application/json")]
        Json,
        [Value("application/x-www-form-urlencoded")]
        Urlencoded
    }
}
