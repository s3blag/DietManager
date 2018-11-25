using DM.Models.ViewModels;
using System;
using System.Threading.Tasks;

namespace DM.Logic.Interfaces
{
    public interface IImageService
    {
        Task<Guid> AddImageAsync(string base64Image);
        Task<ImageVM> GetImageByIdAsync(Guid imageId);
        Task<bool> DeleteImageAsync(Guid imageId);
    }
}