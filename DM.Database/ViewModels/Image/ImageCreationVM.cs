using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels.Image
{
    public class ImageCreationVM
    {
        [Required]
        public string Image { get; set; }
    }
}
