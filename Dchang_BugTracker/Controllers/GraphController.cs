using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dchang_BugTracker.Models;

namespace Dchang_BugTracker.Controllers
{

    public class GraphController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //GET: Graph
        public JsonResult ProduceChart1Data()
        {
            var myData = new List<ChartData>();
            ChartData data = null;
            foreach (var priority in db.TicketPriorities.ToList())
            {
                data = new ChartData();
                data.label = priority.PriorityName;
                data.value = db.Tickets.Where(t => t.TicketPriority.PriorityName == priority.PriorityName).Count();
                myData.Add(data);
            }
            return Json(myData);
        }



        public JsonResult ProduceChart2Data()
        {
            var myData = new List<ChartData>();
            ChartData data = null;
            foreach (var status in db.TicketStatus.ToList())
            {
                data = new ChartData();
                data.label = status.StatusName;
                data.value = db.Tickets.Where(t => t.TicketStatus.StatusName == status.StatusName).Count();
                myData.Add(data);
            }
            return Json(myData);
        }




        public JsonResult ProduceChart3Data()
        {
            var myData = new List<ChartData>();
            ChartData data = null;
            foreach (var type in db.TicketTypes.ToList())
            {
                data = new ChartData();
                data.label = type.TypeName;
                data.value = db.Tickets.Where(t => t.TicketType.TypeName == type.TypeName).Count();
                myData.Add(data);
            }
            return Json(myData);
        }
    }
}