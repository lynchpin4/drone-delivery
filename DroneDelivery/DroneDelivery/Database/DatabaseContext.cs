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
    [DbConfigurationType(typeof(DatabaseConfiguration))] 
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<InventoryRequest> InventoryRequests { get; set; }
        public DbSet<Drone> Drones { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLocation> OrderLocations { get; set; }

        static DatabaseContext()
        {
            System.Data.Entity.Database.SetInitializer<DatabaseContext>(new DatabaseInitializer());
        }

        //.. finally.
        public const string AZURE_DB = "Server=tcp:f9w1mr9fv7.database.windows.net,1433;Database=DroneDelivery;User ID=DroneDelivery@f9w1mr9fv7;Password=ic3d-windows-db2014;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";

        public DatabaseContext()
            : base(AZURE_DB)
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