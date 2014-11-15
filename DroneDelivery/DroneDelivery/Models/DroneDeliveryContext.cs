using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DroneDelivery.Models
{
    public class DroneDeliveryContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        // This file is used for nothing except scaffolding. The other context doesn't work.
    
        public DroneDeliveryContext() : base("name=DroneDeliveryContext")
        {
        }

        public System.Data.Entity.DbSet<DroneDelivery.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<DroneDelivery.Models.InventoryTransaction> InventoryTransactions { get; set; }
    
    }
}
