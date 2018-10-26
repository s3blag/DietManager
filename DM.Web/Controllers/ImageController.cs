using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DM.Logic.Interfaces;
using DM.Models.ViewModels.Image;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/")]
    public class ImageController : Controller
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(Guid id)
        {
            var request = Request;
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            byte[] image = await _imageService.GetImageByIdAsync(id);

            if (image == null)
                return NotFound("Image with the given id was not found.");

            return Ok (Encoding.UTF8.GetString(image));
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddImage([FromBody] ImageVM image)
        {
            if (String.IsNullOrEmpty(image.Image))
            {
                return BadRequest();
            }

            byte[] byteImage = Encoding.UTF8.GetBytes(image.Image);

            return Ok (await _imageService.AddImageAsync(byteImage));
        }

    }
}