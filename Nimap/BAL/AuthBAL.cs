using Microsoft.IdentityModel.Tokens;
using Nimap.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Nimap.BAL
{
    public class AuthBAL
    {
        private static string Secret = ConfigurationSettings.AppSettings["SecretKey"];

        public static string GenerateToken(UserModel user)
        {
            byte[] key = Convert.FromBase64String(Secret);

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.Role , user.Role.ToString()));

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);

            HttpContext httpContext = HttpContext.Current;
            httpContext.Session["UserToken"] = handler.WriteToken(token);

            return handler.WriteToken(token);
        }

       

        public string GetKey()
        {
            System.Security.Cryptography.HMACSHA256 hmac = new System.Security.Cryptography.HMACSHA256();
            return Convert.ToBase64String(hmac.Key);
        }
    }
}