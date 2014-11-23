using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DroneDelivery.Models;
using DroneDelivery.Database;

namespace DroneDelivery.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class OrdersAdminController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: OrdersAdmin
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.OrderLocation).Include(o => o.Product).Include(o => o.User);
            return View(orders.ToList());
        }

        // GET: OrdersAdmin/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: OrdersAdmin/Create
        public ActionResult Create()
        {
            ViewBag.OrderLocationId = new SelectList(db.OrderLocations, "Id", "Address");
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: OrdersAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,OrderedAt,OrderLocationId,ProductId,DroneId,OrderStatus")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.Id = Guid.NewGuid();
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderLocationId = new SelectList(db.OrderLocations, "Id", "Address", order.OrderLocationId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName", order.ProductId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", order.UserId);
            return View(order);
        }

        // GET: OrdersAdmin/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderLocationId = new SelectList(db.OrderLocations, "Id", "Address", order.OrderLocationId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName", order.ProductId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", order.UserId);
            return View(order);
        }

        // POST: OrdersAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,OrderedAt,OrderLocationId,ProductId,DroneId,OrderStatus")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderLocationId = new SelectList(db.OrderLocations, "Id", "Address", order.OrderLocationId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName", order.ProductId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", order.UserId);
            return View(order);
        }

        // GET: OrdersAdmin/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: OrdersAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
