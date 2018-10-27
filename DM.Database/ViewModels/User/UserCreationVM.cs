using System.ComponentModel.DataAnnotations;

namespace DM.Logic.ViewModels
{
    public class UserCreationVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        [MinLength(4)]
        public string UserName { get; set; }

    }
}
