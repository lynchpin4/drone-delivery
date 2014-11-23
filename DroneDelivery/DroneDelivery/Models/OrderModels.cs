using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DroneDelivery.Models
{
    public enum OrderStatus
    {
        // initial status, product may not be in stock
        CREATED = 1,

        // the order is pending delivery
        PENDING = 2,

        // made it
        DELIVERED = 3,

        // the drone is moving
        TRANSIT = 4,

        // canceled (return drone to base, return product to inventory)
        CANCELED = 5,

        // awaiting a return to stock (availabilty check on order omitted due to time)
        AWAITING_STOCK = 6
    }

    /// <summary>
    /// A single order, with customer information and a destination location.
    /// </summary>
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public DateTime OrderedAt { get; set; }

        /// <summary>
        /// Who the order is for
        /// </summary>
        public virtual ApplicationUser User { get; set; }

        public int OrderLocationId { get; set; }

        /// <summary>
        /// The destination for the order
        /// </summary>
        public virtual OrderLocation OrderLocation { get; set; }

        public int ProductId { get; set; }

        /// <summary>
        /// The product we will deliver
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        /// The drone dispatched to deliver the product
        /// </summary>
        public virtual Drone Drone { get; set; }
        public Guid DroneId { get; set; }

        /// <summary>
        /// Enum status of the order
        /// </summary>
        public OrderStatus OrderStatus { get; set; }
    }

    /// <summary>
    /// The location of a delivery in a domestic area. This class will validate that a given address/zip is within the delivery range
    /// and fill in the lat/lng from a 3rd party API.
    /// </summary>
    public class OrderLocation
    {
        public int Id { get; set; }

        public OrderLocation()
        {
            Destination = new DroneLocation();
            HasDestination = false;
        }

        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        /// <summary>
        /// A rough destination (landing loc) of the drone. In a production application of this nature
        /// I'd allow the user to choose the exact point on the map where they'd like the drone to land
        /// with the map initially set on the location with the address/zip
        /// </summary>
        public DroneLocation Destination { get; set; }
        public bool HasDestination { get; set; }
    }
}