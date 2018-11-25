using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class AvatarVM
    {
        [Required]
        public Guid ImageId { get; set; }
    }
}
