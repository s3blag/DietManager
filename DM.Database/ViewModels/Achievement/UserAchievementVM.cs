using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{

    public class UserAchievementVM
    {
        public Guid? Id { get; set; }

        public string Category { get; set; }

        public string Type { get; set; }

        public int Value { get; set; }

        public bool? Seen { get; set; }
    }
}
