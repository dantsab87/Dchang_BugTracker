using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dchang_BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace Dchang_BugTracker.Helper
{
    public class TicketHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper();
        private ProjectHelper projectHelper = new ProjectHelper();

        public int SetDefaultTicketStatus() 
        {
            return db.TicketStatus.FirstOrDefault(ts => ts.StatusName == "Open").Id;
        }

        public int AssignedTicketStatus()
        {
            return db.TicketStatus.FirstOrDefault(ts => ts.StatusName == "Assigned").Id;
        }

        public int SetDefaultTicketPriority() 
        {
            return db.TicketPriorities.FirstOrDefault(tp => tp.PriorityName == "None").Id;
        }

        public List<Ticket> ListMyTickets()
        {
            var myTickets = new List<Ticket>();
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();

            //if (myRole == "Admin")
            //{
            //    myTickets.AddRange(db.Tickets);
            //}
            //else if (myRole == "Project Manager")
            //{
            //    myTickets.AddRange(user.Projects.SelectMany(p => p.Tickets));
            //}
            //else if (myRole == "Developer")
            //{
            //    myTickets.AddRange(db.Tickets.Where(t => t.OwnerUserId == userId));
            //}
            //else if (myRole == "Submitter")
            //{
            //    myTickets.AddRange(db.Tickets.Where(t => t.AssignedToUserId == userId));
            //}

            switch (myRole)
            {
                case "Admin":
                case "Demo Admin":
                    myTickets.AddRange(db.Tickets);
                    break;
                case "Project Manager":
                case "Demo Project Manager":
                    myTickets.AddRange(user.Projects.SelectMany(p => p.Tickets));
                    break;
                case "Developer":
                case "Demo Developer":
                    myTickets.AddRange(db.Tickets.Where(t => t.AssignedToUserId == userId));
                    break;
                case "Submitter":
                case "Demo Submitter":
                    myTickets.AddRange(db.Tickets.Where(t => t.OwnerUserId == userId));
                    break;
                default:
                    break;
            }

            return myTickets;
        }



    }
}