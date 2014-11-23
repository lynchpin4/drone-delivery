using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DroneDelivery.Models
{
    /// <summary>
    /// The status of a drone
    /// </summary>
    public enum DroneStatus
    {
        HOME = 1, // drone is at home base, ready to make a delivery
        OFFLINE = 2, // drone hasn't checked in in a while..
        RECHARGING = 3, // drone is home but shouldn't take off, battery is low

        // flight status
        INFLIGHT = 4,
        LANDING = 5,
        TAKEOFF = 6,
        APPROACH = 7 // so as to notify the customer when the drone is close to arriving
    }

    /// <summary>
    /// The database model representing a drone.
    /// </summary>
    public class Drone
    {
        public Drone()
        {
            Location = DroneBaseStation.BaseLocation;
            PetName = "Artificial Drone";
            Battery = 100.0;
            Status = DroneStatus.HOME;
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Drone unique identifier
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// The friendly name of the drone
        /// </summary>
        public string PetName { get; set; }

        /// <summary>
        /// The drone's battery reading (factors in to which drone is used for delivery)
        /// </summary>
        public double Battery { get; set; }

        /// <summary>
        /// Status of the drone
        /// </summary>
        public DroneStatus Status { get; set; }

        /// <summary>
        /// The current order the drone is handling.
        /// </summary>
        public virtual Order CurrentOrder { get; set; }
        
        /// <summary>
        /// The current location of the drone. This property is a complex type.
        /// </summary>
        public DroneLocation Location { get; set; }

        /// <summary>
        /// The drone's last checkin, updated for every API call from the drone.
        /// </summary>
        public DateTime LastUpdate { get; set; }
    }

    /// <summary>
    /// The default home base for the drone, the geocoded address of the Seattle Amazon HQ.
    /// </summary>
    public class DroneBaseStation
    {
        public static DroneLocation BaseLocation = new DroneLocation()
        {
            Latitude = 47.622279,
            Longitude = -122.336829
        };

        public DroneBaseStation()
        {
            // Home base (Amazon HQ - 410 Terry Ave N Seattle WA)
            Location = BaseLocation;
        }
        public DroneLocation Location { get; set; }
    }

    /// <summary>
    /// A complex type representing location on the globe.
    /// </summary>
    [ComplexType]
    public class DroneLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }

        public string ToString()
        {
            return Latitude + "," + Longitude;
        }
    }

}