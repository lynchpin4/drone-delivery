using DroneDelivery.Database;
using DroneDelivery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DroneDelivery.Controllers_Api
{
    public class InventoryRequestResult : InventoryRequest
    {
        public InventoryRequestResult() : base()
        {
            Message = "";
            Error = false;
        }

        public string Message { get; set; }
        public bool Error { get; set; }
    }

    [RoutePrefix("api/inventory")]
    public class InventoryController : ApiController
    {
        protected DatabaseContext db = new DatabaseContext();

        /// <summary>
        /// Check the availability of a product for purchase by ID, the demo app will automatically respond
        /// when the check count reaches 5 (for the demo purposes of being non-atomic) the request will
        /// respond with the current inventory count.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("check/{id:int}")]
        public InventoryRequestResult Check(int id)
        {
            var request = db.InventoryRequests.Create();
            request.CreatedAt = DateTime.Now;
            request.ProductId = id;
            request.CheckCount = 1;
            request.HasResult = false;

            db.InventoryRequests.Add(request);
            db.SaveChanges();

            return request as InventoryRequestResult;
        }

        [Route("check/{id:guid}")]
        public InventoryRequestResult Check(Guid id)
        {
            var request = (from r in db.InventoryRequests where r.Id.Equals(id) select r).SingleOrDefault();
            if (request != null)
            {
                request.CheckCount++;

                InventoryRequestResult resp = request as InventoryRequestResult;
                if (request.CheckCount >= 5)
                {
                    resp.StockCount = Inventory.GetTotalInStock(db, resp.ProductId);
                    resp.InStock = false;
                    if (resp.StockCount > 0)
                        resp.InStock = true;

                    resp.Message = "OK";
                    resp.Error = false;
                    resp.HasResult = true;
                }

                db.SaveChanges();
                return resp;
            }
            else
            {
                return new InventoryRequestResult() { Error = true, Message = "request expired / deleted" };
            }
        }

    }
}
