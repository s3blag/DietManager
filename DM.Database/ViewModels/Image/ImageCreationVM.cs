using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels.Image
{
    public class ImageCreationVM
    {
        [Required]
        [MaxLength(20000000)]
        public string Image { get; set; }
    }
}
