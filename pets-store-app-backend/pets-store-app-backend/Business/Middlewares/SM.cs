using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Encyption;
using Core.Utilities.Security.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public class SM
    {
        private RequestDelegate _next;


        private CookieOptions accessTokenCookieOptions = new CookieOptions
        {
            Expires = DateTime.UtcNow.AddMinutes(30),
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.Strict
        };

        public SM(RequestDelegate next)
        {
            _next = next;

        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            Console.WriteLine(httpContext);
            await _next(httpContext);
            return;
        }
    }
}
