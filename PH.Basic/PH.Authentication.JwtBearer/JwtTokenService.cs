using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PH.Authentication.JwtBearer.Options;
using PH.ToolsLibrary.Reflection;

namespace PH.Authentication.JwtBearer
{
    public class JwtTokenService: IJwtTokenService
    {
        private JwtSetting JwtSetting { get; set; }
        private JwtSecurityTokenHandler JwtSecurityTokenHandler { get; set; }
        public JwtTokenService(IOptions<JwtSetting> options)
        {
            JwtSetting = options.Value;
            JwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        /// <summary>
        /// 生成 token
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="jwtSetting"></param>
        /// <returns></returns>
        public  string GenerateToken(object obj,JwtSetting jwtSetting = null) 
        {
            jwtSetting = jwtSetting ?? JwtSetting;

           var dics = obj.GetPropertieValues();

            //组装claims
            var claims = dics.Select(x => new Claim(x.Key,x.Value?.ToString())).ToList();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecurityKey));
            var credentials = new SigningCredentials(key, string.IsNullOrWhiteSpace(jwtSetting.Algorithms) ? SecurityAlgorithms.HmacSha256 : jwtSetting.Algorithms);

            var jwtSecurityToken = new JwtSecurityToken(jwtSetting.Issuer,jwtSetting.Audience,claims,null,DateTime.Now.AddMinutes(jwtSetting.ExpiredTime),credentials);
            
            return JwtSecurityTokenHandler.WriteToken(jwtSecurityToken);
        }

        /// <summary>
        /// get claims by token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public IEnumerable<Claim> GetClaims(string token) 
        {
           var tokenHandler = new JwtSecurityTokenHandler();
            
           return tokenHandler.ReadJwtToken(token)?.Claims;
        }

        /// <summary>
        /// 手动验证token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool ValidateToken(string token, TokenValidationParameters tokenValidationParameters = null) 
        {
            tokenValidationParameters = tokenValidationParameters ?? JwtSetting.TokenValidationParameters;
            SecurityToken validatedToken = null;
            try
            {
                JwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
            }
            catch (SecurityTokenException stexp)
            {
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
            //... manual validations return false if anything untoward is discovered
            return validatedToken != null;
        }
    }
}
