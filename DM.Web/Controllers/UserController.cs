using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Logic.Interfaces;
using DM.Models.ViewModels;
using DM.Models.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
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

            var loggedUserId = Guid.Empty;

            var result = await _searchService.SearchUsersAsync(loggedUserId, lastReturned);

            if (!result.Result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}