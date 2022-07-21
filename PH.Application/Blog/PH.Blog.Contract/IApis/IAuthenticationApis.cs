using PH.Blog.Contract.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Blog.Contract.IApis
{
    public interface IAuthenticationApis
    {
        /// <summary>
        /// 通过账号密码登陆
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<LoginDto> LoginByAccountandPasswordAsync(LoginInput input);
    }
}
