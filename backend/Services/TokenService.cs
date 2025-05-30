using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Todo.Interfaces;
using Todo.Models;

namespace Todo.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            _config = config;

            var key = _config["JWT:SigningKey"];
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidOperationException("Отсутствует ключ JWT:SigningKey");
            }

            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }

        public string CreateToken(AppUser appUser)
        {
            var appUserEmail =
                appUser.Email
                ?? throw new InvalidOperationException("У пользователя отсутствует Email");

            var appUserName =
                appUser.UserName
                ?? throw new InvalidOperationException("У пользователя отсутствует Username");

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Email, appUserEmail),
                new(JwtRegisteredClaimNames.GivenName, appUserName),
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"],
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
