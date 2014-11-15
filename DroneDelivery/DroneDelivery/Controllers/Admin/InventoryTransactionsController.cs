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
    public class InventoryTransactionsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: InventoryTransactions
        public async Task<ActionResult> Index()
        {
            var inventoryTransactions = db.InventoryTransactions.Include(i => i.Product);
            return View(await inventoryTransactions.OrderByDescending((x) => x.Id).ToListAsync());
        }

        // GET: InventoryTransactions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryTransaction inventoryTransaction = await db.InventoryTransactions.FindAsync(id);
            if (inventoryTransaction == null)
            {
                return HttpNotFound();
            }
            return View(inventoryTransaction);
        }
        
        // GET: InventoryTransactions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryTransaction inventoryTransaction = await db.InventoryTransactions.FindAsync(id);
            if (inventoryTransaction == null)
            {
                return HttpNotFound();
            }
            return View(inventoryTransaction);
        }

        // POST: InventoryTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            InventoryTransaction inventoryTransaction = await db.InventoryTransactions.FindAsync(id);
            db.InventoryTransactions.Remove(inventoryTransaction);
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
