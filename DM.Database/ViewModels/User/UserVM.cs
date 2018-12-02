using System;
using System.ComponentModel.DataAnnotations;

namespace DM.Models.ViewModels
{
    public class UserVM
    {
        public Guid? Id { get; set; }

        public Guid? ImageId { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string City { get; set; }

        public bool? IsFriend { get; set; }
    }
}
