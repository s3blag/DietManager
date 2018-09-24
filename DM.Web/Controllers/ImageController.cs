using System;
using System.Text;
using System.Threading.Tasks;
using DM.Logic.Interfaces;
using DM.Models.ViewModels.Image;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Controllers
{
    [Route("api/[controller]/[action]")]
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
            if (id == Guid.Empty)
            {
                return BadRequest();
            }
            return Ok (await _imageService.GetImageByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddImage([FromBody] ImageVM image)
        {
            if (String.IsNullOrEmpty(image.Image))
            {
                return BadRequest();
            }

            byte[] byteImage = Encoding.ASCII.GetBytes(image.Image);

            return Ok (await _imageService.AddImageAsync(byteImage));
        }

    }
}