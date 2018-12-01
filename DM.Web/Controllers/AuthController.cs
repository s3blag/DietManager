using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DM.Logic.Interfaces;
using DM.Models.Config;
using DM.Models.Models;
using DM.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DM.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly SecuritySettings _securitySettings;

        public AuthController(IUserService userService, IOptions<SecuritySettings> options)
        {
            _userService = userService;
            _securitySettings = options.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginVM model)
        {
            var user1 = User;
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid login data.");
            }

            var user = await _userService.GetUserByLoginDataAsync(model);

            if (user == null)
            {
                return BadRequest("Invalid username or password.");
            }

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

            var results = new AuthToken
            {
                Value = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };

            var loggedInUser = new LoggedInUserVM(user, results);

            return Ok(loggedInUser);
            
        }
    }
}