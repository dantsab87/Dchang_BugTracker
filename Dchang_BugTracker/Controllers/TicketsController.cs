using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Dchang_BugTracker.Helper;
using Dchang_BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace Dchang_BugTracker.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private TicketHelper ticketHelper = new TicketHelper();
        private RoleHelper roleHelper = new RoleHelper();
        private TicketHistoryHelper tkHistory = new TicketHistoryHelper();
        private NotificationHelper notificationHelper = new NotificationHelper();

        // GET: Tickets
        public ActionResult Index()
        {
            //var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            //return View(tickets.ToList());
            return View(ticketHelper.ListMyTickets());
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Project Manager, Demo Project Manager, Developer, Demo Developer, Submitter, Demo Submitter")]
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var projects = user.Projects;

            ViewBag.xProject = new SelectList(projects, "Id", "Name");
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName");
            //ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "PriorityName");
            //ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "StatusName");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "TypeName");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Project Manager, Demo Project Manager, Developer, Demo Developer, Submitter, Demo Submitter")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectId,TicketTypeId,Title,Description")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {

                ticket.Created = DateTime.Now;
                ticket.OwnerUserId = User.Identity.GetUserId();
                ticket.TicketStatusId = ticketHelper.SetDefaultTicketStatus();
                ticket.TicketPriorityId = ticketHelper.SetDefaultTicketPriority();
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index", "Tickets");
            }

            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var projects = user.Projects;
            ViewBag.xProject = new SelectList(projects, "Id", "Name", ticket.ProjectId);
            //ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            //ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            //ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "PriorityName", ticket.TicketPriorityId);
            //ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "StatusName", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "TypeName", ticket.TicketTypeId);

            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Admin, Demo Admin, Project Manager, Demo Project Manager, Developer, Demo Developer, Submitter, Demo Submitter")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }

            RoleHelper helper = new RoleHelper();
            var users = helper.UsersIn2Role("Developer", "Demo Developer").ToList();
            ViewBag.AssignedToUserId = new SelectList(users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "PriorityName", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "StatusName", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "TypeName", ticket.TicketTypeId);
            return View(ticket);


        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, Demo Admin, Project Manager, Demo Project Manager, Developer, Demo Developer, Submitter, Demo Submitter")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectId,TicketPriorityId,TicketTypeId,TicketStatusId,OwnerUserId,AssignedToUserId,Title,Description,Created,Updated")] Ticket ticket)
        {
            //if (User.IsInRole("Submitter") || (User.IsInRole("Demo Submitter")))
            //{
            //    var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
            //}
            if (ModelState.IsValid)
            {
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);


                ticket.Updated = DateTime.Now;
                db.Entry(ticket).State = EntityState.Modified;


                db.SaveChanges();


                var newTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

                tkHistory.RecordHistoricalChanges(oldTicket, newTicket);

                //decide for yourself NotificationHelper if a Notification needs to be generated

                notificationHelper.ManageNotifications(oldTicket, newTicket);

                return RedirectToAction("Index");
            }
            RoleHelper helper = new RoleHelper();
            var users = helper.UsersIn2Role("Developer", "Demo Developer").ToList();
            ViewBag.AssignedToUserId = new SelectList(users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "PriorityName", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "StatusName", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "TypeName", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Admin, Demo Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [Authorize(Roles = "Admin, Demo Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        // GET Tickets/Assign Tickets
        [Authorize(Roles = "Admin, Demo Admin, Project Manager, Demo Project Manager")]
        public ActionResult AssignTicket(int? id) 
        {
            RoleHelper helper = new RoleHelper();
            var ticket = db.Tickets.Find(id);
            var users = helper.UsersIn2Role("Developer", "Demo Developer").ToList();
            ViewBag.AssignedToUserId = new SelectList(users, "Id", "DisplayName", ticket.AssignedToUserId);

            return View(ticket);
        }

        //POST Tickets/Assign Tickets
        [Authorize(Roles = "Admin, Demo Admin, Project Manager, Demo Project Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AssignTicket(Ticket model) 
        {

            var ticket = db.Tickets.Find(model.Id);

            ticket.AssignedToUserId = model.AssignedToUserId;
            ticket.TicketStatusId = ticketHelper.AssignedTicketStatus();

            if (ticket.AssignedToUserId == null)
            {
                ticket.AssignedToUserId = model.AssignedToUserId = null;
                ticket.TicketStatusId = ticketHelper.SetDefaultTicketStatus();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {

                //ticket.AssignedToUserId = model.AssignedToUserId;
                //ticket.TicketStatusId = ticketHelper.AssignedTicketStatus();
                ticket.Updated = DateTime.Now;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();




                var callbackUrl = Url.Action("Details", "Tickets", new { id = ticket.Id }, protocol: Request.Url.Scheme);

                try
                {
                    EmailService ems = new EmailService();
                    IdentityMessage msg = new IdentityMessage();
                    ApplicationUser user = db.Users.Find(model.AssignedToUserId);

                    msg.Body = $"You have been assigned a Ticket.{Environment.NewLine} Please click the following link to view the details <a href=\"{callbackUrl}\">NEW TICKET</a>";
                    msg.Destination = user.Email;
                    msg.Subject = "You've been assigned a Ticket";

                    await ems.SendMailAsync(msg);
                }
                catch (Exception ex)
                {
                    await Task.FromResult(0);
                }

                return RedirectToAction("Index");

            }
        }

    }
}
