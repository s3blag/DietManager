using System.Linq;
using System.Threading.Tasks;
using DM.Logic.Interfaces;
using DM.Models.ViewModels;
using DM.Models.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly ISearchService _searchService;

        public UserController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchUsers([FromBody] IndexedResult<UserSearchVM> lastReturned)
        {
            if (lastReturned != null && lastReturned.IsLast)
            {
                return NotFound("Invalid arguments");
            }

            var result = await _searchService.SearchUsersAsync(lastReturned);

            if (!result.Result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }

      
    }
}