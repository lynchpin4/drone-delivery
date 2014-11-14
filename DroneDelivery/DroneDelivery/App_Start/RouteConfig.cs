using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DroneDelivery.Controllers_Api;
using DroneDelivery.Controllers.Admin;

namespace DroneDelivery
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            // Web page controllers (older MVC routing method)
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Products",
                url: "{controller}/{action}/id",
                defaults: new { controller = "Products", action = "Index", id = UrlParameter.Optional }
            );

            /*
            // Admin web page controllers
            routes.MapRoute(
                name: "AdminHome",
                url: "admin/{controller}/{action}",
                defaults: new { controller = "AdminHome", action = "Index" },
                namespaces: new string[] { "DroneDelivery.Controllers.Admin" }
            );
             */
        }
    }
}
