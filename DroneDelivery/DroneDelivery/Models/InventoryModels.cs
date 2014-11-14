using DroneDelivery.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DroneDelivery.Models
{
    /// <summary>
    /// The inventory class itself 
    /// </summary>
    public class Inventory
    {

        /// <summary>
        /// Get total of all items in the inventory/stock, by product id.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static int GetTotalInStock(DatabaseContext db, int productId)
        {
            int total = db.InventoryTransactions.Where((tx) => tx.ProductId == productId).Sum((x) => (int?)x.Count) ?? 0;
            return total;
        }

    }

    // Transactional Inventory Counting
    public class InventoryTransaction
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        // Who caused this transaction
        public bool IsUser { get; set; }
        public bool IsAdmin { get; set; }

        public DateTime OccurredAt { get; set; }

        // A positive or negative number representing x of product removed or added.
        public int Count { get; set; }
    }

    /// <summary>
    /// Since the challenge states the inventory request should be 'non atomic' an inventory request will be created when the user
    /// attempts to purchase a product. An API will provide a way to check the status of a given inventory request. In a production
    /// scenario this could be deleted based on the CreationTime key or when the purchase transaction is completed.
    /// </summary>
    public class InventoryRequest
    {
        public Guid Id { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        /// <summary>
        /// The number of times this request has been checked by the client
        /// </summary>
        [IgnoreDataMember]
        public int CheckCount { get; set; }

        /// <summary>
        /// Do we have an inventory result yet? This field makes it extremely simple to check on the client.
        /// </summary>
        public bool HasResult { get; set; }

        // The rest of the properties are filled after the request has been fufilled.
        public int? StockCount { get; set; }

        public bool? InStock { get; set; }
    }
}