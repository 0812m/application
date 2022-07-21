using Microsoft.IdentityModel.Tokens;
using PH.Authentication.JwtBearer.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PH.Authentication.JwtBearer
{
    public interface IJwtTokenService
    {
        string GenerateToken(object obj, JwtSetting jwtSetting = null);

        IEnumerable<Claim> GetClaims(string token);

        bool ValidateToken(string token, TokenValidationParameters tokenValidationParameters = null);
    }
}
