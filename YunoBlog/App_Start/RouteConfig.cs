using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace YunoBlog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Calendar",
                url: "{y}/{m}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                constraints: new { y = @"\d+", m = @"\d+" }
            );
            routes.MapRoute(
                name: "Category",
                url: "Category/{c}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                constraints: new { c = @"\d+" }
            ); 
            routes.MapRoute(
                 name: "Search",
                 url: "Search/{s}",
                 defaults: new { controller = "Home", action = "Index" }
             );
            routes.MapRoute(
                name: "Page",
                url: "Page/{id}",
                defaults: new { controller = "Page", action = "Show", id = UrlParameter.Optional },
                constraints: new { id = @"\d+" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
