﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dchang_BugTracker.Models
{
    public class ProfileViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string AvatarPath { get; set; }

    }


}