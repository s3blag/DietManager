using System;

namespace DM.Models.ViewModels
{
    public class LoggedInUserVM: UserVM
    {
        public DateTimeOffset? TokenExpirationDate { get; set; }
    }
}
