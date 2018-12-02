using System.Threading.Tasks;
using DM.Logic.Interfaces;
using DM.Models.ViewModels;
using DM.Web.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Controllers
{
    [ModelStateValidator]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISecurityService _securityService;

        public AuthController(IUserService userService, ISecurityService securityService)
        {
            _userService = userService;
            _securityService = securityService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginVM model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid login data.");
            }

            var user = await _userService.GetUserByLoginDataAsync(model);

            if (user == null)
            {
                return BadRequest("Invalid username or password.");
            }

            bool passwordCorrect = _securityService.VerifyPassword(model.Password, user.Password);

            if (!passwordCorrect)
            {
                return BadRequest("Invalid username or password.");
            }

            var authToken = _securityService.GenerateAuthToken(user);

            var loggedInUser = new LoggedInUserVM(user, authToken);

            return Ok(loggedInUser);

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserCreationVM model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid user creation data.");
            }

            model.Password = _securityService.EncryptPassword(model.Password);

            var user = await _userService.CreateUserAsync(model);

            if (user == null)
            {
                return BadRequest("Username is not unique");
            }

            var authToken = _securityService.GenerateAuthToken(user);

            var loggedInUser = new LoggedInUserVM(user, authToken);

            return Ok(loggedInUser);
        }
        
    }
}