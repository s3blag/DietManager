using LinqToDB.Identity;
using System;

namespace DM.Models.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public Guid PhotoId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

    }
}
