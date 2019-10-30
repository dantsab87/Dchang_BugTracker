using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dchang_BugTracker.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Dashboard() 
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Profile()
        {
            ViewBag.Message = "Your Profile.";

            return View();
        }

        public ActionResult EditProfile()
        {
            ViewBag.Message = "Your Profile.";

            return View();
        }

        public ActionResult UserSettings()
        {
            ViewBag.Message = "Your Settings.";

            return View();
        }
    }
}