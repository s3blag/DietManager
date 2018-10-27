using System.Threading.Tasks;

namespace DM.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<bool> AddAsync(T model);
        Task<bool> DeleteAsync(T model);
    }
}