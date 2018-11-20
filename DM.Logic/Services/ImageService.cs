using AutoMapper;
using DM.Database;
using DM.Logic.Interfaces;
using DM.Models.Config;
using DM.Models.Exceptions;
using DM.Models.Models;
using DM.Models.ViewModels;
using DM.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DM.Logic.Services
{
    public class ImageService : IImageService
    {
        private readonly ImageServiceConfig _config; 
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;

        public ImageService(IImageRepository imageRepository, IMapper mapper, IOptions<ImageServiceConfig> options)
        {
            _imageRepository = imageRepository;
            _mapper = mapper;
            _config = options.Value;
        }

        public async Task<ImageVM> GetImageByIdAsync(Guid imageId)
        {
            ValidateArgument((imageId, nameof(imageId)));

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
            ValidateArgument((base64Image, nameof(base64Image)));

            var imageCreation = await CreateFullImagePathAsync(_config.ImagesRootDirectoryPath, GetExtension(base64Image));

            //compress image

            await WriteImageAsync(imageCreation, GetBase64Image(base64Image));

            var dbImage = _mapper.Map<Image>(imageCreation);

            bool imagePathSaved = await _imageRepository.AddAsync(dbImage);

            if (!imagePathSaved)
            {
                throw new DataAccessException($"Image path for guid: {dbImage} could not be saved!");
            }

            return dbImage.Id;
        }

        private string GetExtension(string base64Image)
        {
            var match = Regex.Match(base64Image, Constants.Base64ExtensionRegex);

            return "." + match.Groups["type"].Value;
        }

        private string GetBase64Image(string fullBase64Image)
        {
            return fullBase64Image.Substring(fullBase64Image.LastIndexOf(',') + 1);
        }

        private async Task<Byte[]> ReadImageAsync(string imagePath) =>
             await File.ReadAllBytesAsync(imagePath);


        private async Task WriteImageAsync(ImageCreation imageCreation, string base64Image) => 
            await File.WriteAllBytesAsync(imageCreation.Path, base64Image.Select(x => (byte)x).ToArray());

        private async Task<Byte[]> CompressImage(Byte[] image)
        {
            throw new NotImplementedException();
        }

        private async Task<ImageCreation> CreateFullImagePathAsync(string basePath, string extension)
        {
            int count = await _imageRepository.CountAsync();

            int imageInternalIndex = count % (_config.AmountOfImagesInSubDirectory - 1);

            bool createDirectory = imageInternalIndex == 0;

            string newImageDirectoryPath = Path.Combine(basePath, (count / (_config.AmountOfImagesInSubDirectory - 1)).ToString());

            if (createDirectory)
            {
                await Task.Run(() =>
                {
                    Directory.CreateDirectory(newImageDirectoryPath);
                });  
            }

            return new ImageCreation(Path.Combine(newImageDirectoryPath, imageInternalIndex.ToString() + extension));
        }
        
        private void ValidateArgument(params (Object value, string name)[] arguments)
        {
            foreach (var (value, name) in arguments)
            {
                if (value is null)
                {
                    throw new ArgumentNullException(name);
                }
            }
        }
    }
}
