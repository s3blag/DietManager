using System.Threading.Tasks;
using DM.Logic.Interfaces;
using DM.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public AuthController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
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

            var authToken = _authService.GenerateAuthToken(user);

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

            var user = await _userService.CreateUserAsync(model);

            if (user == null)
            {
                return BadRequest("Username is not unique");
            }

            var authToken = _authService.GenerateAuthToken(user);

            var loggedInUser = new LoggedInUserVM(user, authToken);

            return Ok(loggedInUser);
        }
        
    }
}