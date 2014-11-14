using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DroneDelivery.Controllers.Admin
{
    [RoutePrefix("admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        [Route("test")]
        public ActionResult Test()
        {
            return Content("<p>hey this worked. gr8</p>");
        }

        [Route("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}