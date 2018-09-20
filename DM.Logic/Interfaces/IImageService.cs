using System;
using System.Threading.Tasks;

namespace DM.Logic.Interfaces
{
    public interface IImageService
    {
        Task<Guid> AddImageAsync(byte[] image);
        Task<byte[]> GetImageByIdAsync(Guid imageId);
    }
}