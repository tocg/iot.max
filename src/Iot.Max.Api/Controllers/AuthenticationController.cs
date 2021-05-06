using Iot.Max.Model.Models.Token;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Api.Controllers
{
    /// <summary>
    /// 身份认证
    /// </summary>
    [Route("auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private TokenParameter _tokenParameter = new TokenParameter();
        /// <summary>
        /// 
        /// </summary>
        public AuthenticationController()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            _tokenParameter = config.GetSection("tokenParameter").Get<TokenParameter>();
        }


        /// <summary>
        /// //请求token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("requestToken")]
        //[Microsoft.AspNetCore.Authorization.Authorize(Roles = "testUser")]
        public IActionResult RequestToken(LoginRequestDTO request)
        {
            //这儿在做用户的帐号密码校验。这儿略过了。
            if (request.username == null && request.password == null)
                return BadRequest("Invalid Request");

            //生成Token和RefreshToken
            var token = GenUserToken(request.username, "testUser");
            var refreshToken = "123456";

            return Ok(new { token, refreshToken });
        }

        /// <summary>
        /// //刷新token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("refreshToken")]
        public ActionResult RefreshToken(RefreshTokenDTO request)
        {
            if (request.Token == null && request.RefreshToken == null)
                return BadRequest("Invalid Request");

            //这儿是验证Token的代码
            var handler = new JwtSecurityTokenHandler();
            try
            {
                ClaimsPrincipal claim = handler.ValidateToken(request.Token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenParameter.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                }, out SecurityToken securityToken);

                var username = claim.Identity.Name;

                //这儿是生成Token的代码
                var token = GenUserToken(username, "testUser");

                var refreshToken = "654321";

                return Ok(new[] { token, refreshToken });
            }
            catch (Exception)
            {
                return BadRequest("Invalid Request");
            }
        }


        //生成token
        private string GenUserToken(string username, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenParameter.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                _tokenParameter.Issuer,
                null,
                claims,
                expires: DateTime.UtcNow.AddMinutes(_tokenParameter.AccessExpiration),
                signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return token;
        }
    }
}
