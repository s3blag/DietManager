using System;
using System.IO;
using System.Threading.Tasks;
using DM.Logic.Interfaces;
using DM.Models.ViewModels.Image;
using DM.Web.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace DM.Web.Controllers
{
    [Route("api/[controller]/")]
    public class ImageController : Controller
    {
        private readonly IImageService _imageService;
        private readonly IContentTypeProvider _contentTypeProvider;

        public ImageController(IImageService imageService, IContentTypeProvider contentTypeProvider)
        {
            _imageService = imageService;
            _contentTypeProvider = contentTypeProvider;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var image = await _imageService.GetImageByIdAsync(id);

            if (image == null)
                return NotFound("Image with the given id was not found.");

            if (!_contentTypeProvider.TryGetContentType(image.Extension, out var contentType))
            {
                throw new IOException("Content type could not be evaluated from file extension.");
            }

            return File(image.Content, contentType);
        }

        [Authorize]
        [HttpPost("add")]
        [ModelStateValidator]
        public async Task<IActionResult> AddImage([FromBody] ImageCreationVM image)
        {
            if (String.IsNullOrEmpty(image.Image))
            {
                return BadRequest();
            }

            return Ok (await _imageService.AddImageAsync(image.Image));
        }

    }
}