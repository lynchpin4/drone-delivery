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
    public class CreateDroneRequest
    {

    }

    /// <summary>
    /// Updates sent from drone
    /// </summary>
    public class UpdateDroneRequest
    {
        public Guid Id { get; set; }
        public double Battery { get; set; }
        public DroneStatus Status { get; set; }
        public DroneLocation Location { get; set; }
    }

    /// <summary>
    /// Due to time constraints / demo, no restrictions are placed on this api
    /// which would obviously be neccessary in a real app.
    /// 
    /// Drones self register in the system when they come online. Their default
    /// location is the home base configured in the app.
    /// </summary>
    public class DronesController : ApiController
    {
        protected DatabaseContext db = new DatabaseContext();

        // GET: api/Drones
        public IEnumerable<Drone> Get()
        {
            return (from d in db.Drones select d).ToList();
        }

        // GET: api/Drones/xxx
        public Drone Get(Guid id)
        {
            return (from d in db.Drones where d.Id == id select d).FirstOrDefault();
        }

        [HttpPost]
        public Drone CreateOrUpdateDrone()
        {
            return new Drone();
        }

        /// <summary>
        /// Update from the drone about it's status.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public bool UpdateStatus(UpdateDroneRequest request)
        {
            var drone = (from d in db.Drones where d.Id == request.Id select d).FirstOrDefault();
            if (drone != null)
            {
                drone.Battery = request.Battery;
                drone.Status = request.Status;
                drone.Location = request.Location;
                drone.LastUpdate = DateTime.Now;

                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
