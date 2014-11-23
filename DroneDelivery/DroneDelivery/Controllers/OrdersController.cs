using DroneDelivery.Database;
using DroneDelivery.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DroneDelivery.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        public DatabaseContext db = new DatabaseContext();
        protected UserStore<ApplicationUser> store;

        public OrdersController()
        {
            store = new UserStore<ApplicationUser>(db);
        }
        
        /// <summary>
        /// Create Order for the given product ID, this is the page where
        /// the user inputs their address
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            int productId = int.Parse(Request["id"]);
            ViewData.Add("product", (from p in db.Products where p.Id == productId select p).First());
            return View();
        }

        [HttpPost]
        public ActionResult CreateOrder()
        {
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(store);

            var order = db.Orders.Create();
            order.Id = Guid.NewGuid();
            order.ProductId = int.Parse(Request["ProductId"]);

            order.UserId = User.Identity.Name;
            order.OrderStatus = OrderStatus.CREATED;
            order.OrderedAt = DateTime.Now;
            
            order.OrderLocation = new OrderLocation()
            {
                HasDestination = true,

                Address = Request["Address"],
                ZipCode = Request["ZipCode"],
                State = Request["State"],
                City = Request["City"],

                Destination = new DroneLocation()
                {
                    Latitude = double.Parse(Request["latitude"]),
                    Longitude = double.Parse(Request["longitude"])
                }
            };

            db.Orders.Add(order);
            db.SaveChanges();

            return RedirectToAction("Display", order.Id);
        }
        
        // it would be a good idea to authenticate the order belongs to the user in question here but since this is a demo i'll skip that.
        public ActionResult Display()
        {
            var id = Guid.Parse(Request["id"]);
            return View((from p in db.Orders where p.Id == id select p).First());
        }

        /// <summary>
        /// Check availability of the given product for the given order. (For concurrency purposes and simplification we don't do anything special like reserve the product before the order is created)
        /// If the product is no longer available the user could be notified when it is back in stock.
        /// </summary>
        /// <returns></returns>
        public ActionResult Check(int orderId)
        {
            return View();
        }
    }
}