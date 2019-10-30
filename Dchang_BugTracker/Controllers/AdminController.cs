using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dchang_BugTracker.Helper;
using Dchang_BugTracker.Models;

namespace Dchang_BugTracker.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper();
        private ProjectHelper projHelper = new ProjectHelper();
        // GET: Admin
        public ActionResult ManageRoles()
        {
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email");
            ViewBag.Role = new SelectList(db.Roles, "Name", "Name");

            var users = new List<ManageRolesViewModel>();
            foreach (var user in db.Users.ToList()) 
            {
                users.Add(new ManageRolesViewModel
                {
                    UserName = $"{user.LastName}, {user.FirstName}",
                    RoleName = roleHelper.ListUserRoles(user.Id).FirstOrDefault()
                });
            }

            return View(users);
        }


        // POST: Admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageRoles(List<string> userIds, string role)
        {
            //Unenroll all the selected Users from ANY roles
            //they may currently occupy
            foreach (var userId in userIds) 
            {
                //What is the Role of this person??
                var userRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
                if (userRole != null) 
                {
                    roleHelper.RemoveUserFromRole(userId, userRole);
                }
            }

            //Step 2: Add them back to the seleced Role
            if (!string.IsNullOrEmpty(role))
            {
                foreach (var userId in userIds)
                {
                    roleHelper.AddUserToRole(userId, role);
                }
            }
            return RedirectToAction("ManageRoles", "Admin");
        }


        [Authorize(Roles= "Admin, Project Manager")]
        public ActionResult ManageProjectUsers() 
        {
            ViewBag.Projects = new MultiSelectList(db.Projects, "Id", "Name");
            ViewBag.Developers = new MultiSelectList(roleHelper.UsersInRole("Developer"), "Id", "Email");
            ViewBag.Submitters = new MultiSelectList(roleHelper.UsersInRole("Submitter"), "Id", "Email");

            if (User.IsInRole("Admin")) 
            {
                ViewBag.ProjectManagerId = new SelectList(roleHelper.UsersInRole("Project Manager"), "Id", "Email");
            }

            //Lets create a View Model for purposes of displaying User's and their Associated Projects
            var myData = new List<UserProjectListViewModel>();
            UserProjectListViewModel userVm = null;
            foreach (var user in db.Users.ToList()) 
            {
                userVm = new UserProjectListViewModel
                {
                    Name = $"{user.LastName}, {user.FirstName}",
                    ProjectNames = projHelper.ListUserProjects(user.Id).Select(p => p.Name).ToList()
                };

                if (userVm.ProjectNames.Count() == 0) 
                {
                    userVm.ProjectNames.Add("N/A");
                }

                myData.Add(userVm);
            }
            
            return View(myData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProjectUsers(List<int> projects, string projectManagerId, List<string> developers, List<string> submitters) 
        {
            //Remove users from every project I have selected
            if (projects != null) 
            {
                foreach (var projectId in projects) 
                {
                    //Remove everyone from THIS project
                    foreach (var user in projHelper.UsersOnProject(projectId).ToList()) 
                    {
                        projHelper.RemoveUserFromProject(user.Id, projectId);
                    }

                    //Add back a PM if I can
                    if (!string.IsNullOrEmpty(projectManagerId))
                    {
                        projHelper.AddUserToProject(projectManagerId, projectId);
                    }

                    if (developers != null) 
                    {
                        foreach (var developerId in developers) 
                        {
                            projHelper.AddUserToProject(developerId, projectId);
                        }
                    }

                    if (submitters != null)
                    {
                        foreach (var submitterId in submitters)
                        {
                            projHelper.AddUserToProject(submitterId, projectId);
                        }
                    }
                }
            }
            return RedirectToAction("ManageProjectUsers");
        }
    }


}