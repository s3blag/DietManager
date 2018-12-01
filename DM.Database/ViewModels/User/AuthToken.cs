using System;
using System.Collections.Generic;
using System.Text;

namespace DM.Models.ViewModels
{
    public class AuthToken
    {
        public string Value { get; set; }

        public DateTime Expiration { get; set; }
    }
}
