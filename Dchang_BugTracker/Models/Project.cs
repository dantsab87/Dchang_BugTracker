﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dchang_BugTracker.Models;

namespace Dchang_BugTracker.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }


        //Nav section...(child)
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> Users {get; set;}


        public Project() 
        {
            Tickets = new HashSet<Ticket>();
            Users = new HashSet<ApplicationUser>();
        }
    }
}