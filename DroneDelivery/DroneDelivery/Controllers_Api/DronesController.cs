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
    public class DronesController : ApiController
    {
        protected DatabaseContext db = new DatabaseContext();

        // GET: api/Drones
        public IEnumerable<Drone> Get()
        {
            return (from d in db.Drones select d).ToList();
        }

        // GET: api/Drones/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Drones
        public void Post([FromBody]string value)
        {

        }

        // PUT: api/Drones/5
        public void Put(Guid id, [FromBody]string value)
        {
        }

        // DELETE: api/Drones/5
        public void Delete(int id)
        {
        }
    }
}
