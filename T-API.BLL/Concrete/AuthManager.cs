using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using T_API.BLL.Abstract;
using T_API.Core.Settings;

namespace T_API.BLL.Concrete
{
    public class AuthManager:IAuthService
    {
        private IHttpContextAccessor _httpContextAccessor;

        public AuthManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Login()
        {
            return GenerateToken();
        }
        private string GenerateToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(ConfigurationSettings.SecretKey);
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, "test"));
            claims.Add(new Claim(ClaimTypes.Email, "berkay.yalcin20@hotmail.com"));
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));



            var signingCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            var claimsIdentity = new ClaimsIdentity(claims);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.Now.AddYears(10),
                SigningCredentials = signingCredentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            //_httpContextAccessor.HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));
            return tokenHandler.WriteToken(token);
        }

    }
}