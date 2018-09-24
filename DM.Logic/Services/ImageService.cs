using AutoMapper;
using DM.Database;
using DM.Logic.Interfaces;
using DM.Models.Config;
using DM.Models.ViewModels;
using DM.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.IO;
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

        public async Task<byte[]> GetImageByIdAsync(Guid imageId)
        {
            if (imageId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(imageId));
            }

            var imageMetaData = await _imageRepository.GetImageByIdAsync(imageId);

            byte[] image = await ReadImageAsync(imageMetaData.Path);

            return image;
        }

        public async Task<Guid> AddImageAsync(byte[] image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            //TODO Read from config file
            var imageCreation = await CreateFullImagePathAsync(_config.ImagesRootDirectoryPath);

            //compress image

            await WriteImageAsync(imageCreation, image);

            var dbImage = _mapper.Map<Image>(imageCreation);

            bool imagePathSaved = await _imageRepository.AddImageAsync(dbImage);

            if (!imagePathSaved)
            {
                throw new Exception($"Image path for guid: {dbImage} could not be saved!");
            }

            return dbImage.Id;
        }

        private async Task<Byte[]> ReadImageAsync(string imagePath) =>
             await File.ReadAllBytesAsync(imagePath);


        private async Task WriteImageAsync(ImageCreation imageCreation, byte[] image) => 
            await File.WriteAllBytesAsync(imageCreation.Path, image);

        private async Task<Byte[]> CompressImage(Byte[] image)
        {
            throw new NotImplementedException();
        }

        private async Task<ImageCreation> CreateFullImagePathAsync(string basePath)
        {
            int count = await _imageRepository.CountAsync();

            int imageInternalIndex = count % (_config.AmountOfImagesInSubDirectory - 1);

            bool createDirectory = imageInternalIndex == 0;

            string newImageDirectoryPath = Path.Combine(basePath, (count / (_config.AmountOfImagesInSubDirectory - 1)).ToString());

            if (createDirectory)
            {
                await Task.Run(() =>
                {
                    var directoryInfo = Directory.CreateDirectory(newImageDirectoryPath);
                });  
            }

            return new ImageCreation(Path.Combine(newImageDirectoryPath, imageInternalIndex.ToString() + ".jpg"));
        }
    }
}
