using DM.Models.Enums;
using System;

namespace DM.Models.Models
{
    public class ActivityCreation
    {
        public Guid UserId { get; set; }

        public Guid ContentId { get; set; }

        public ActivityType ActivityType { get; set; }
    }
}
