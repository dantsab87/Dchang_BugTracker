using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dchang_BugTracker.Models
{
    public class TicketStatus
    {
        public int Id { get; set; }
        public string StatusName { get; set; }

        //Nav..(child)
        public virtual ICollection<Ticket> Tickets { get; set; }

        public TicketStatus()
        {
            Tickets = new HashSet<Ticket>();
        }    
    }
}