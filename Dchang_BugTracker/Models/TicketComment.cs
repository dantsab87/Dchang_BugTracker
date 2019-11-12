using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Dchang_BugTracker.Models;

namespace Dchang_BugTracker.Models
{
    public class TicketComment
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Comment")]
        [StringLength(500, MinimumLength = 3, ErrorMessage = "Comment must have a minimum length of 3 and max length of 500.")]
        public string CommentBody { get; set; }
        public DateTime Created { get; set; }
        public int TicketId { get; set; }
        public string UserId { get; set; }

        //Nav
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}