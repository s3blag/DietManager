using System;

namespace DM.Models.ViewModels
{
    public class FriendActivityVM
    {
        public UserVM Friend { get; set; }
        public object Activity { get; set; }
        public DateTimeOffset ActivityDateTime { get; set; }
    }
}
