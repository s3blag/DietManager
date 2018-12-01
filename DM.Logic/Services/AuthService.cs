using DM.Logic.Interfaces;
using DM.Models.Config;
using DM.Models.ViewModels;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DM.Logic.Services
{
    public class AuthService : IAuthService
    {
        private readonly SecuritySettings _securitySettings;

        public AuthService(IOptions<SecuritySettings> options)
        {
            _securitySettings = options.Value;
        }

        public AuthToken GenerateAuthToken(UserVM user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securitySettings.Key));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _securitySettings.Issuer,
                Audience = _securitySettings.Audience,
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = credentials
            };

            var token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);

            var tokenWithExpDate = new AuthToken
            {
                Value = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };

            return tokenWithExpDate;
        }
    }
}
