using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DroneDelivery.Models;
using DroneDelivery.Database;

namespace DroneDelivery.Controllers.Admin
{
    [RoutePrefix("admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        protected DatabaseContext db = new DatabaseContext();

        [Route("products")]
        public ActionResult Products()
        {
            var model = new { Products = new List<Product>() };
            model.Products.AddRange((from p in db.Products select p).ToList());
            return View(model);
        }

        [Route("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}