using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SCNDISC.Server.Core.Models;
using SCNDISC.Server.Core.Models.AuthOptions;
using SCNDISC.Server.Core.Models.User;

namespace SCNDISC.Server.Core.Controllers
{
    public class AccountController: ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string userName;
        private readonly string pass;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
            userName = _configuration.GetValue<string>("DefaultUserName");
            pass = _configuration.GetValue<string>("DefaultPassword");
        }
        [HttpPost]
        [Route("token")]
        public async Task Token([FromBody] LoginModel model)
        {
                var identity = GetIdentity(model.Login, model.Password);
                if (identity == null)
                {
                    Response.StatusCode = 401;
                    await Response.WriteAsync("Invalid login or password.");
                    return;
                }

                var now = DateTime.UtcNow;
                // create JWT-token
                var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                var response = new 
                {
                    access_token = encodedJwt,
                    username = identity.Name
                };

                Response.ContentType = "application/json";
                await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            if (userName == username && pass == password)
            {
                var admin = new User { Login = userName, Password = pass, Role = Roles.AdminRole };
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, admin.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, admin.Role)
                };
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }
    }

}
