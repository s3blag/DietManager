using AutoMapper;
using DM.Database;
using DM.Models.Config;
using DM.Web;
using LinqToDB.Data;
using Microsoft.Extensions.Options;

namespace DM.Tests
{
    public static class ObjectsFactory
    {
        public static IMapper GetMapperInstance()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            Mapper.Configuration.CompileMappings();

            return Mapper.Instance;
        }

        public static void InitDbConnection(string connectionString) =>
            DataConnection.DefaultSettings = new DBConnectionSettings(connectionString);

        public static IOptions<ImageServiceConfig> GetImageServiceIOptions(int numberOfImagesInSubdirectory, string imagesRootDirectory) =>
            Options.Create(new ImageServiceConfig() { AmountOfImagesInSubDirectory = numberOfImagesInSubdirectory, ImagesRootDirectoryPath = imagesRootDirectory });
    }
}
