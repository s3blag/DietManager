using DM.Database;
using LinqToDB;
using System;
using System.Threading.Tasks;

namespace DM.Repositories.Repositories
{
    public class ImageRepository
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
    }
}
