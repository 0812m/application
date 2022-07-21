using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PH.Blog.Contract;
using PH.Blog.Contract.Dtos;
using PH.Blog.Contract.IApis;
using PH.Blog.Contract.IServices;
using PH.Web.Core;
using PH.Web.Core.Cache;
using PH.Web.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using PH.Web.Core.Authentication;

namespace PH.Blog.Application.Apis
{
    [AuthorizeApp, Route("api/auth"), ApiDescriptionSettings("用户授权模块")]
    public class AuthenticationApis : BaseApiController,IAuthenticationApis
    {
        private readonly IAuthenticationSvc _authSvc;
        private readonly IJwtTokenService _jwtSvc;

        public AuthenticationApis(IAuthenticationSvc authSvc,IJwtTokenService jwtSvc,IConfigurtionSvc configurtionSvc, IHttpContextAccessor httpContextAccessor, ICacheProvide cache) 
            : base(configurtionSvc, httpContextAccessor, cache)
        {
            _authSvc = authSvc;
            _jwtSvc = jwtSvc;
        }

        /// <summary>
        /// 通过账号密码登陆
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [AllowAnonymous,Route("login")]
        public async Task<LoginDto> LoginByAccountandPasswordAsync(LoginInput input)
        {
            var user = await _authSvc.GetByAccountAsync(input.Account);

            if (user is null)
                throw Sorry.Bad(ErrorCodes.UserNotExist);
            var hash = input.Password.MD5Encrypt(Encoding.UTF8, 32);
            if (!string.Equals(hash,user.PasswordHash, StringComparison.OrdinalIgnoreCase))
                throw Sorry.Bad(ErrorCodes.AccountOrPasswordIsIncorrect);
            if (user.IsDeleted)
                throw Sorry.Bad(ErrorCodes.InvalidUser);

            var obj = new Identity()
            {
                UserId = user.Id,
                NickName = user.NickName,
                Description = user.Description,
                CreateTime = user.CreateTime,
                RoleJoin = string.Join(',', user.Roles.Select(x => x.Code))
            };

           var token =  _jwtSvc.GenerateToken(obj);
            return new LoginDto() 
            {
                Token = token
            };
        }
    }
}
