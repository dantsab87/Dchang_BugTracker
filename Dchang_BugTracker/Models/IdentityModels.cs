using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Dchang_BugTracker.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "First Name")] 
        [StringLength(50, MinimumLength = 3, ErrorMessage ="First Name must have a minimum length of 3 and max length of 50." )]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "First Name must have a minimum length of 3 and max length of 50.")]
        public string LastName { get; set; }

        [Display(Name = "Display Name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "First Name must have a minimum length of 3 and max length of 50.")]
        public string DisplayName { get; set; }

        public virtual ICollection<TicketComment> TicketComments { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<TicketAttachment> TicketAttachments { get; set; }
        public virtual ICollection<TicketHistory> TicketHistories { get; set; }
        public virtual ICollection<TicketNotification> TicketNotifications { get; set; }

        //I am intentionally NOT including ICollections to the Ticket...
        public ApplicationUser() 
        {
            TicketComments = new HashSet<TicketComment>();
            Projects = new HashSet<Project>();
            TicketAttachments = new HashSet<TicketAttachment>();
            TicketHistories = new HashSet<TicketHistory>();
            TicketNotifications = new HashSet<TicketNotification>();        
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Dchang_BugTracker.Models.Project> Projects { get; set; }

        public System.Data.Entity.DbSet<Dchang_BugTracker.Models.Ticket> Tickets { get; set; }

        public System.Data.Entity.DbSet<Dchang_BugTracker.Models.TicketPriority> TicketPriorities { get; set; }

        public System.Data.Entity.DbSet<Dchang_BugTracker.Models.TicketStatus> TicketStatus { get; set; }

        public System.Data.Entity.DbSet<Dchang_BugTracker.Models.TicketType> TicketTypes { get; set; }

        public System.Data.Entity.DbSet<Dchang_BugTracker.Models.TicketAttachment> TicketAttachments { get; set; }

        public System.Data.Entity.DbSet<Dchang_BugTracker.Models.TicketComment> TicketComments { get; set; }

        public System.Data.Entity.DbSet<Dchang_BugTracker.Models.TicketHistory> TicketHistories { get; set; }

        public System.Data.Entity.DbSet<Dchang_BugTracker.Models.TicketNotification> TicketNotifications { get; set; }
    }
}