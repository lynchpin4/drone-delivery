using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DroneDelivery.Models;
using DroneDelivery.Database;

namespace DroneDelivery.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class ProductsAdminController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: ProductsAdmin
        public async Task<ActionResult> Index()
        {
            var products = await db.Products.ToListAsync();
            return View(products);
        }

        // GET: ProductsAdmin/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id) as Product;
            if (product == null)
            {
                return HttpNotFound();
            }

            ViewData.Add("current_stock", Inventory.GetTotalInStock(db, product.Id));
            return View(product);
        }

        // GET: ProductsAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductsAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ProductName,ProductImageURL,ProductDescription,ProductCategory,ProductPrice,ProductWeight")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                await db.SaveChangesAsync();

                // Put at least one in the stock when a new product gets created.
                var p = product as Product;
                Inventory.AddToStock(db, p.Id, 1);

                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: ProductsAdmin/ChangeStock/5
        public async Task<ActionResult> ChangeStock(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id) as Product;
            if (product == null)
            {
                return HttpNotFound();
            }

            ViewData.Add("current_stock", Inventory.GetTotalInStock(db, product.Id));
            return View(product);
        }

        // POST: ProductsAdmin/ChangeStock/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeStock()
        {
            var id = int.Parse(Request["id"]);

            // int.tryparse todo.
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = await db.Products.FindAsync(id) as Product;
            //product.Stock = ;
            var stock = int.Parse(Request["stock_count"]);
            if (stock != 0)
                Inventory.AddToStock(db, product.Id, stock);

            ViewData.Add("current_stock", Inventory.GetTotalInStock(db, product.Id));

            if (product == null)
            {
                return HttpNotFound();
            }

            ViewData.Add("success", "Inventory Stock Updated. The current total is displayed below.");
            return View(product);
        }

        // GET: ProductsAdmin/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id) as Product;
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: ProductsAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ProductName,ProductImageURL,ProductDescription,ProductCategory,ProductPrice,ProductWeight")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: ProductsAdmin/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: ProductsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
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
