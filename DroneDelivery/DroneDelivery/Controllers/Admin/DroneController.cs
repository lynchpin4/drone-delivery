using DroneDelivery.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DroneDelivery.Controllers.Admin
{
    [Authorize(Roles="Admin")]
    public class DroneController : Controller
    {
        protected DatabaseContext db = new DatabaseContext();

        // GET: Drone
        public ActionResult Index(int id)
        {
            return View();
        }

        public ActionResult Drones()
        {
            return View((from d in db.Drones select d));
        }
    }
}