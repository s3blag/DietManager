using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels.Image
{
    public class ImageVM
    {
        [Required]
        public string Image { get; set; }
    }
}
