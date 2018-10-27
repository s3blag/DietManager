using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class UserSearchVM
    {
        [Required]
        public string Query { get; set; }
    }
}
