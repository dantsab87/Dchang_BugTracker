using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dchang_BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace Dchang_BugTracker.Helper
{
    public class TicketHistoryHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void RecordHistoricalChanges(Ticket oldTicket, Ticket newTicket) 
        {
            //compare a few properties from the old and new ticket to determin if changes were made

            if (oldTicket.TicketStatusId != newTicket.TicketStatusId) 
            {
                var newHistoryRecord = new TicketHistory
                {
                    Property = "TicketStatusId",
                    OldValue = oldTicket.TicketStatus.StatusName,
                    NewValue = newTicket.TicketStatus.StatusName,
                    Changed = (DateTime)newTicket.Updated,
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId()
                };

                db.TicketHistories.Add(newHistoryRecord);
            }

            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                var newHistoryRecord = new TicketHistory
                {
                    Property = "TicketPriorityId",
                    OldValue = oldTicket.TicketPriority.PriorityName,
                    NewValue = newTicket.TicketPriority.PriorityName,
                    Changed = (DateTime)newTicket.Updated,
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId()
                };

                db.TicketHistories.Add(newHistoryRecord);
            }




            if (oldTicket.AssignedToUserId != newTicket.AssignedToUserId)
            {
                var newHistoryRecord = new TicketHistory
                {
                    Property = "DeveloperId",
                    OldValue = oldTicket.AssignedToUser == null? "UnAssigned" : oldTicket.AssignedToUser.FirstName,
                    NewValue = newTicket.AssignedToUser == null? "UnAssigned" : newTicket.AssignedToUser.FirstName,
                    Changed = (DateTime)newTicket.Updated,
                    TicketId = newTicket.Id,
                    UserId = HttpContext.Current.User.Identity.GetUserId()
                };

                db.TicketHistories.Add(newHistoryRecord);
            }





            db.SaveChanges();
        }

    }
}