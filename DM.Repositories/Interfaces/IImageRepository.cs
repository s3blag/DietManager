using System;
using System.Threading.Tasks;
using DM.Database;

namespace DM.Repositories.Interfaces
{
    public interface IImageRepository
    {
        Task<bool> AddImageAsync(Image image);
        Task<Image> GetImageByIdAsync(Guid id);
        Task<int> CountAsync();
    }
}