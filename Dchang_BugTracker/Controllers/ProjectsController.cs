using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dchang_BugTracker.Helper;
using Dchang_BugTracker.Models;

namespace Dchang_BugTracker.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper();
        private ProjectHelper projectHelper = new ProjectHelper();

        public ActionResult ManageUsers(int id) 
        {
            ViewBag.ProjectId = id;

            #region PM section
            //string currentPm = null;
            //foreach (var user in projectHelper.UsersOnProject(id)) 
            //{

            //    if (roleHelper.IsUserInRole(user.Id, "Project Manager")) 
            //    {
            //        currentPm = user.Id;
            //    }
            //}
            var pmId = projectHelper.ListUsersOnProjectInRole(id, "Project Manager").FirstOrDefault();
            ViewBag.ProjectManagerId = new SelectList(roleHelper.UsersInRole("Project Manager"), "Id", "Email", pmId);
            #endregion

            #region Dev section
            //var projDevs = new List<string>();
            //foreach (var user in projectHelper.UsersOnProject(id))
            //{

            //    if (roleHelper.IsUserInRole(user.Id, "Developer"))
            //    {
            //        projDevs.Add(user.Id);
            //    }
            //}
            ViewBag.Developers = new MultiSelectList(roleHelper.UsersInRole("Developer"), "Id", "Email", projectHelper.ListUsersOnProjectInRole(id, "Developer"));
            #endregion

            #region Subs section
            //var projSubs = new List<string>();
            //foreach (var user in projectHelper.UsersOnProject(id))
            //{

            //    if (roleHelper.IsUserInRole(user.Id, "Submitters"))
            //    {
            //        projSubs.Add(user.Id);
            //    }
            //}
            ViewBag.Submitters = new MultiSelectList(roleHelper.UsersInRole("Submitter"), "Id", "Email", projectHelper.ListUsersOnProjectInRole(id, "Submitter"));
            #endregion

            return View(db.Projects.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUsers(int projectId, string projectManagerId, List<string> developers, List<string> submitters) 
        {
            foreach (var user in projectHelper.UsersOnProject(projectId).ToList()) 
            {
                projectHelper.RemoveUserFromProject(user.Id, projectId);
            }

            //In order to ensure that I always have the correct and current set of assign users 
            //I will first remove all users from the selected project, then I will add back the selected users
            if (!string.IsNullOrEmpty(projectManagerId)) 
            {
                projectHelper.AddUserToProject(projectManagerId, projectId);
            }

            if (developers != null)
            {
                foreach (var deveolperId in developers)
                {
                    projectHelper.AddUserToProject(deveolperId, projectId);
                }
            }

            if (submitters != null)
            {
                foreach (var submitterId in submitters)
                {
                    projectHelper.AddUserToProject(submitterId, projectId);
                }
            }

            return RedirectToAction("ManageUsers", new { id = projectId});
        }



        // GET: Projects
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Demo Admin, Project Manager, Demo Project Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Created,Updated")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Demo Admin, Project Manager, Demo Project Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Created,Updated")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [Authorize(Roles = "Admin, Demo Admin, Project Manager, Demo Project Manager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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
    }
}
