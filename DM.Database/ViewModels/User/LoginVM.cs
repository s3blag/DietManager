using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class LoginVM
    {
        [Required]
        [MinLength(2)]
        public string Username { get; set; }

        [Required]
        [MinLength(2)]
        public string Password { get; set; }
    }
}
