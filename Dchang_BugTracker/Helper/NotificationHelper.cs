using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dchang_BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace Dchang_BugTracker.Helper
{
    public class NotificationHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        public void ManageNotifications(Ticket oldTicket, Ticket newTicket)
        {
            var ticketHasBeenAssigned = oldTicket.AssignedToUserId == null && newTicket.AssignedToUserId != null;
            var ticketHasBeenUnassigned = oldTicket.AssignedToUserId != null && newTicket.AssignedToUserId == null;
            var ticketHasBeenReassigned = oldTicket.AssignedToUserId != null && newTicket.AssignedToUserId != null && oldTicket.AssignedToUserId != newTicket.AssignedToUserId;

            if (ticketHasBeenAssigned)
                AddAssignmentNotification(oldTicket, newTicket);
            else if (ticketHasBeenUnassigned)
                AddUnassignmentNotification(oldTicket, newTicket);
            else if (ticketHasBeenReassigned) 
            {
                AddAssignmentNotification(oldTicket, newTicket);
                AddUnassignmentNotification(oldTicket, newTicket);
            }

        }

        private void AddAssignmentNotification(Ticket oldTicket, Ticket newTicket) 
        {
            var notification = new TicketNotification
            {
                TicketId = newTicket.Id,
                IsRead = false,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                RecipientId = newTicket.AssignedToUserId,
                Created = DateTime.Now,
                NotificationBody = $"You have been assigned to Ticket Id: {newTicket.Id} on Project: {newTicket.Project.Name}. The Ticket Title is '{newTicket.Title}' and was created on {newTicket.Created} with a priority of {newTicket.TicketPriority.PriorityName}"
            };
            db.TicketNotifications.Add(notification);
            db.SaveChanges();
        }

        private void AddUnassignmentNotification(Ticket oldTicket, Ticket newTicket) 
        {
            var notification = new TicketNotification
            {
                TicketId = newTicket.Id,
                IsRead = false,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                RecipientId = oldTicket.AssignedToUserId,
                Created = DateTime.Now,
                NotificationBody = $"You have been unassigned to Ticket Id: {newTicket.Id} on Project: {newTicket.Project.Name}. The Ticket Title is '{newTicket.Title}' and was created on {newTicket.Created} with a priority of {newTicket.TicketPriority.PriorityName}"
            };
            db.TicketNotifications.Add(notification);
            db.SaveChanges();
        }

        public static List<TicketNotification> GetUnreadNotifications() 
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
           
            if (userId != null) 
            {
                return db.TicketNotifications.Include("Sender").Include("Recipient").Where(t => t.RecipientId == userId && !t.IsRead).ToList();
                
            }
            return new List<TicketNotification>();
        }

    }
}