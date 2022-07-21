using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.FileStorage.Ftp
{
    public class FtpOptions
    {
        public virtual string User { get; set; }

        public virtual string Password { get; set; }

        public virtual string IP { get; set; }

        public virtual string StorageRootPath { get; set; }

        public virtual string SavePath { get; set; }

        public virtual bool EnableSsl { get; set; }

        public virtual bool KeepAlive { get; set; }

        public virtual bool UseBinary { get; set; }

        public virtual string Url { get { return $"ftp://{IP}/{StorageRootPath}/{SavePath}"; } }

        public FtpOptions()
        {
          
        }
    }
}
