using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class UserCreationVM
    {
        [Required]
        [MinLength(4)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string PasswordRepeated { get; set; }

        [Required]
        [MinLength(4)]
        public string Username { get; set; }

        [Required]
        [MinLength(2)]
        public string Name { get; set; }

        [Required]
        [MinLength(2)]
        public string Surname { get; set; }

        [Required]
        [MinLength(2)]
        public string City { get; set; }


    }
}
