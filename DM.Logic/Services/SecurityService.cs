using DM.Logic.Interfaces;
using DM.Models.Config;
using DM.Models.ViewModels;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DM.Logic.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly SecuritySettings _securitySettings;

        public SecurityService(IOptions<SecuritySettings> options)
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
                Expires = DateTime.UtcNow.AddDays(5),
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

        public string EncryptPassword(string password)
        {
            var salt = new byte[Constants.SALT_SIZE];
            new RNGCryptoServiceProvider().GetBytes(salt);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Constants.ITERATIONS_COUNT);
            var hash = pbkdf2.GetBytes(Constants.HASH_SIZE);

            var hashBytes = new byte[Constants.SALT_SIZE + Constants.HASH_SIZE];
            Array.Copy(salt, 0, hashBytes, 0, Constants.SALT_SIZE);
            Array.Copy(hash, 0, hashBytes, Constants.SALT_SIZE, Constants.HASH_SIZE);

            var base64Hash = Convert.ToBase64String(hashBytes);

            return string.Format("$hash${0}${1}", Constants.ITERATIONS_COUNT, base64Hash);
        }

        public bool VerifyPassword(string password, string encryptedPassword)
        {
            var splittedHashString = encryptedPassword.Replace("$hash$", "").Split('$');
            var iterations = int.Parse(splittedHashString[0]);
            var base64Hash = splittedHashString[1];

            var hashBytes = Convert.FromBase64String(base64Hash);

            var salt = new byte[Constants.SALT_SIZE];
            Array.Copy(hashBytes, 0, salt, 0, Constants.SALT_SIZE);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(Constants.HASH_SIZE);

            for (var i = 0; i < Constants.HASH_SIZE; i++)
            {
                if (hashBytes[i + Constants.SALT_SIZE] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
