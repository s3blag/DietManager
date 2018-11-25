using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class AchievementIdsVM
    {
        [Required]
        [MinLength(2)]
        public IEnumerable<Guid> AchievementIds { get; set; }
    }
}
