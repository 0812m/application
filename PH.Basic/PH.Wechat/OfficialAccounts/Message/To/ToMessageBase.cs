using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PH.Wechat.OfficialAccounts.Message.To
{
    [XmlRoot("xml")]
    [Serializable]
    public  class ToMessageBase
    {
        public virtual string ToXml() 
        {
            return "";
        }
    }
}
