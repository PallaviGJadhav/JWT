using Microsoft.IdentityModel.Tokens;
using Nimap.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Nimap.Filters
{
    public class NimapAuthorizeAttribute : AuthorizeAttribute
    {
        private static string Secret = ConfigurationSettings.AppSettings["SecretKey"];

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["UserToken"] != null)
            {
                var authToken = Convert.ToString(httpContext.Session["UserToken"]);

                ClaimsPrincipal principal = GetPrincipal(authToken);

                if (principal == null)
                    return false;


                ClaimsIdentity identity = null;
                try
                {
                    identity = (ClaimsIdentity)principal.Identity;
                }
                catch (NullReferenceException)
                {
                    return false;
                }

                Claim userId = identity.FindFirst(ClaimTypes.Name);
                Claim userRole = identity.FindFirst(ClaimTypes.Role);

                if (Roles.Split(',').Contains(userRole.Value))
                {
                    return true;
                }
            }
            //HandleUnathorized(httpContext);
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary  
               {  
                    { "controller", "Home" },  
                    { "action", "UnAuthorized" }  
               });
        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                
                JwtSecurityToken jwtToken = (JwtSecurityToken) tokenHandler.ReadToken(token);
                if (jwtToken == null)
                {
                    return null;
                }

                byte[] key = Convert.FromBase64String(Secret);
                
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                
                SecurityToken securityToken;
                
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);

                return principal;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}