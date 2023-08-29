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
    public class RefreshTokenMiddleware
    {
        private RequestDelegate _next;
        private IUserService _userService;
        private IAuthService _authService;
        private IConfiguration _configuration;
        private TokenOptions tokenOptions;

        private CookieOptions accessTokenCookieOptions = new CookieOptions
        {
            Expires = DateTime.UtcNow.AddMinutes(30),
        };

        public RefreshTokenMiddleware(RequestDelegate next,IUserService userService,IAuthService authService,IConfiguration configuration)
        {
            _next = next;
            _userService = userService;
            _authService = authService;  
            _configuration = configuration;
            tokenOptions = _configuration.GetSection("TokenOptions").Get<TokenOptions>();
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            
            string refreshToken = httpContext.Request.Cookies["RefreshToken"];
            if (refreshToken == null)
            {
                await _next(httpContext);
                return;
            }

            string accessToken=httpContext.Request.Cookies["AccessToken"];
            if (accessToken != null)
            {
                bool isValid = ValidateToken(accessToken);
                if (isValid)
                {
                    httpContext.Request.Headers.Add("Authorization", "Bearer " + accessToken);
                    await _next(httpContext);
                    return;
                }
            }
            try {
                IDataResult<User> userData = await _userService.GetByRefreshToken(refreshToken);
                IDataResult<AccessToken> _accessToken = await _authService.CreateAccessToken(userData.Data);
                httpContext.Response.Cookies.Append("AccessToken", _accessToken.Data.Token, accessTokenCookieOptions);
            }
            catch (Exception) {
                httpContext.Response.Cookies.Delete("RefreshToken");
                httpContext.Response.Cookies.Delete("AccessToken");
            }
            await _next(httpContext);
            return;   
        }

        private bool ValidateToken(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience,
                    IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
                };
                SecurityToken securityToken;
                tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }


        
        
    }
}
