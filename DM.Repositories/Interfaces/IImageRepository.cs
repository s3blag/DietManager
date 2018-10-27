using System;
using System.Threading.Tasks;
using DM.Database;

namespace DM.Repositories.Interfaces
{
    public interface IImageRepository: IBaseRepository<Image>
    {
        Task<Image> GetImageByIdAsync(Guid id);
        Task<int> CountAsync();
    }
}