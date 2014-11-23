using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DroneDelivery.Models
{
    /// <summary>
    /// Unused DB Context - EF scaffolding doesn't work for the DatabaseContext.
    /// </summary>
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>().HasOptional<Drone>((x) => x.Drone).WithOptionalPrincipal();

            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
        }

        public System.Data.Entity.DbSet<DroneDelivery.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<DroneDelivery.Models.InventoryTransaction> InventoryTransactions { get; set; }

        public System.Data.Entity.DbSet<DroneDelivery.Models.Order> Orders { get; set; }

        public System.Data.Entity.DbSet<DroneDelivery.Models.OrderLocation> OrderLocations { get; set; }

        public System.Data.Entity.DbSet<DroneDelivery.Models.ApplicationUser> ApplicationUsers { get; set; }
    
    }
}
