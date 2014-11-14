using DroneDelivery.Database;
using DroneDelivery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DroneDelivery.Controllers
{
    public class ProductsController : Controller
    {
        protected DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            var products = (from p in db.Products select p).ToList();
            var sorted = new Dictionary<string, List<Product>>();

            foreach (var p in products.OrderBy((p) => p.ProductCategory))
            {
                if (!sorted.ContainsKey(p.ProductCategory))
                    sorted.Add(p.ProductCategory, new List<Product>());

                sorted[p.ProductCategory].Add(p);
            }

            ViewData.Add("products",sorted);
            return View();
        }

        public ActionResult Display(int id)
        {
            var product = (from p in db.Products where p.Id == id select p).FirstOrDefault();
            if (product != null)
            {
                ViewData.Add("product", product);
                return View();
            }
            else
            {
                return Redirect("/products");
            }
        }
    }
}