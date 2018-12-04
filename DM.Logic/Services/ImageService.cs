using AutoMapper;
using DM.Database;
using DM.Logic.Interfaces;
using DM.Models.Config;
using DM.Models.Exceptions;
using DM.Models.Models;
using DM.Models.ViewModels;
using DM.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DM.Logic.Services
{
    public class ImageService : IImageService
    {
        private readonly ImageServiceConfig _config; 
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ImageService> _logger;

        public ImageService(
            IImageRepository imageRepository, 
            IMapper mapper, 
            IOptions<ImageServiceConfig> options, 
            ILogger<ImageService> logger)
        {
            _imageRepository = imageRepository;
            _mapper = mapper;
            _config = options.Value;
            _logger = logger;
        }

        public async Task<ImageVM> GetImageByIdAsync(Guid imageId)
        {
            var imageMetaData = await _imageRepository.GetImageByIdAsync(imageId);

            if (imageMetaData == null)
            {
                return null;
            }

            byte[] image = await ReadImageAsync(imageMetaData.Path);

            return new ImageVM() { Content = image, Extension = Path.GetExtension(imageMetaData.Path) };
        }

        public async Task<Guid> AddImageAsync(string base64Image)
        {
            var imageGuid = Guid.NewGuid();

            var imagePath = await CreateFullImagePathAsync(imageGuid, _config.ImagesRootDirectoryPath, GetExtension(base64Image));

            await WriteImageAsync(imagePath, GetBase64Image(base64Image));

            var dbImage = new Image() { Id = imageGuid, Path = imagePath };

            bool imagePathSaved = await _imageRepository.AddAsync(dbImage);

            if (!imagePathSaved)
            {
                throw new DataAccessException($"Image path for guid: {dbImage} could not be saved!");
            }

            return dbImage.Id;
        }

        public async Task<bool> DeleteImageAsync(Guid imageId)
        {
            var imageMetaData = await _imageRepository.GetImageByIdAsync(imageId);

            if (imageMetaData == null)
            {
                return false;
            }

            bool imageDeleted = await DeleteImageAsync(imageMetaData);

            if (!imageDeleted)
            {
                _logger.LogError("Deleting image with id: {0} and path: {1} failed", imageId, imageMetaData.Path);
                return false;
            }

            return await _imageRepository.DeleteAsync(imageMetaData);
        }

        private string GetExtension(string base64Image)
        {
            var match = Regex.Match(base64Image, Constants.Base64ExtensionRegex);

            return "." + match.Groups["type"].Value;
        }

        private string GetBase64Image(string fullBase64Image) => fullBase64Image.Substring(fullBase64Image.LastIndexOf(',') + 1);

        private async Task<Byte[]> ReadImageAsync(string imagePath) => await File.ReadAllBytesAsync(imagePath);


        private async Task WriteImageAsync(string path, string base64Image) => 
            await File.WriteAllBytesAsync(path, Convert.FromBase64String(base64Image));

        private async Task<bool> DeleteImageAsync(Image image)
        {
            try
            {
                await Task.Run(() => { File.Delete(image.Path); });
                return true;
            }
            catch( Exception ex)
            {
                _logger.LogError(ex, "Deleting an image failed for path {image}", image);
                return false;
            }
        }

        private async Task<string> CreateFullImagePathAsync(Guid guid, string basePath, string extension)
        {
            return Path.Combine(basePath, guid.ToString() + extension);
        }
       
    }
}
