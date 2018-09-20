using AutoMapper;
using DM.Database;
using DM.Logic.Interfaces;
using DM.Models.ViewModels;
using DM.Repositories.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DM.Logic.Services
{
    public class ImageService : IImageService
    {
        private const int AMOUNT_OF_IMAGES_IN_DIRECTORY = 1000;
        //TODO: Read from config file
        private const string IMAGE_BASE_PATH = @"C:\Users\seblag-stacjonarny\Desktop\ImagesRootDirectory";
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;

        public ImageService(IImageRepository imageRepository, IMapper mapper)
        {
            _imageRepository = imageRepository;
            _mapper = mapper;
        }

        public async Task<Byte[]> GetImageByIdAsync(Guid imageId)
        {
            if (imageId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(imageId));
            }

            var imageMetaData = await _imageRepository.GetImageByIdAsync(imageId);

            Byte[] image = await ReadImageAsync(imageMetaData.Path);

            return image;
        }

        public async Task<Guid> AddImageAsync(Byte[] image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            //TODO Read from config file
            var imageCreation = await CreateFullImagePathAsync(IMAGE_BASE_PATH);

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

            int imageInternalIndex = count % (AMOUNT_OF_IMAGES_IN_DIRECTORY - 1);

            bool createDirectory = imageInternalIndex == 0;

            string newImageDirectoryPath = Path.Combine(basePath, (count / (AMOUNT_OF_IMAGES_IN_DIRECTORY - 1)).ToString());

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
