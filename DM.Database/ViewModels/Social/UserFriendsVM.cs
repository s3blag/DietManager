﻿using System;
using System.Collections.Generic;

namespace DM.Models.ViewModels
{
    public class UserFriendsVM
    {
        public Guid UserId { get; set; }

        public IEnumerable<UserVM> Friends { get; set; }
    }
}
