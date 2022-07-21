using PH.Web.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.Dtos
{
    public class LoginInput:Input
    {
        public virtual string Account { get; set; }

        public virtual string Password { get; set; }

        public override void Verification()
        {
            if (string.IsNullOrWhiteSpace(Account))
                throw Sorry.Bad("账号必填！");
            if (string.IsNullOrWhiteSpace(Password))
                throw Sorry.Bad("密码必填！");
        }
    }
}
