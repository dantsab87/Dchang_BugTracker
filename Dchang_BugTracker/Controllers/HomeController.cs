using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dchang_BugTracker.Helper;
using Dchang_BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace Dchang_BugTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper();
        private ProjectHelper projHelper = new ProjectHelper();

       [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard() 
        {
            ViewBag.Role = new SelectList(db.Roles, "Name", "Name");
            return View();
        }

        //GET Home/EditProfile
        public ActionResult EditProfile()
        {

            var sourceUser = db.Users.Find(User.Identity.GetUserId());
            if (sourceUser.AvatarPath == null) 
            {
                sourceUser.AvatarPath = "/Avatar/default.png";
                db.SaveChanges();
            }

            var userVM = new ProfileViewModel();
            userVM.FirstName = sourceUser.FirstName;
            userVM.LastName = sourceUser.LastName;
            userVM.DisplayName = sourceUser.DisplayName;
            userVM.Email = sourceUser.Email;
            userVM.Id = sourceUser.Id;

            return View(userVM);
        }
        //POST Home/EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile([Bind(Include = "Id, FirstName, LastName, DisplayName, Email, AvatarPath")] ProfileViewModel user, HttpPostedFileBase avatar)
        {
            var myuser = db.Users.Find(user.Id);
            myuser.FirstName = user.FirstName;
            myuser.LastName = user.LastName;
            myuser.DisplayName = user.DisplayName;
            myuser.Email = user.Email;
            myuser.UserName = user.Email;

            if (ModelState.IsValid)
            {
                if (myuser.AvatarPath != user.AvatarPath)
                {

                    if (UploadValidator.IsWebFriendlyImage(avatar))
                    {
                        var fileName = Path.GetFileName(avatar.FileName);
                        fileName = StringUtilities.URLFriendly(Path.GetFileNameWithoutExtension(fileName));
                        fileName += "_" + DateTime.Now.Ticks + Path.GetExtension(avatar.FileName);
                        avatar.SaveAs(Path.Combine(Server.MapPath("~/Avatars/"), fileName));
                        myuser.AvatarPath = "/Avatars/" + fileName;



                    }
                }
            }

            db.SaveChanges();
            return RedirectToAction("EditProfile", "Home");

        }

        public ActionResult UserSettings()
        {

            return View();
        }

        public ActionResult Contacts()
        {

            return View();
        }

    }
}