using DroneDelivery.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace DroneDelivery.Database
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<InventoryRequest> InventoryRequests { get; set; }
        public DbSet<Drone> Drones { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLocation> Locations { get; set; }

        static DatabaseContext()
        {
            System.Data.Entity.Database.SetInitializer<DatabaseContext>(new DatabaseInitializer());
        }

        public DatabaseContext()
            : base("DatabaseContext")
        {
            Database.Initialize(false);
        }

        public static DatabaseContext Create()
        {
            System.Data.Entity.Database.SetInitializer<DatabaseContext>(new DatabaseInitializer());
            return new DatabaseContext();
        }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Order>().HasOptional<Drone>(p => p.Drone).WithOptionalPrincipal();

            base.OnModelCreating(modelBuilder);
        }
    }
}