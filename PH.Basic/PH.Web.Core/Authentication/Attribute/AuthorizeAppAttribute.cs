using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Web.Core.Authentication
{
    public class AuthorizeAppAttribute : AuthorizeAttribute
    {
        public AuthorizeAppAttribute()
        {
            Policy = "AppAuthorization";
        }
        public AuthorizeAppAttribute(string policy):base(policy)
        {

        }
    }
}
