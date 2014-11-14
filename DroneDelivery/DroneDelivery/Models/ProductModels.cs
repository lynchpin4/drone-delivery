using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DroneDelivery.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string ProductName { get; set; }
        public string ProductImageURL { get; set; }
        public string ProductDescription { get; set; }
        public string ProductCategory { get; set; }

        /// <summary>
        /// Price shown to the user.
        /// </summary>
        public double ProductPrice { get; set; }

        /// <summary>
        /// The weight of the given product, in dummy units but for all intents and purposes we'll assume lbs.
        /// </summary>
        public double ProductWeight { get; set; }
    }
}