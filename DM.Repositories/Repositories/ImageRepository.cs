using DM.Database;
using DM.Repositories.Interfaces;
using LinqToDB;
using System;
using System.Threading.Tasks;

namespace DM.Repositories
{
    public class ImageRepository : IImageRepository
    {
        public async Task<Image> GetImageByIdAsync(Guid id)
        {
            using (var db = new DietManagerDB())
            {
                var image = await db.Images.
                    FirstOrDefaultAsync(i => i.Id == id);

                return image;
            }
        }

        public async Task<bool> AddImageAsync(Image image)
        {
            using (var db = new DietManagerDB())
            {
                var result = await db.InsertAsync(image);

                return Convert.ToBoolean(result);
            }
        }

        public async Task<int> CountAsync()
        {

            using (var db = new DietManagerDB())
            {
                var count = await db.Images.CountAsync();

                return count;
            }
        }
    }
}
