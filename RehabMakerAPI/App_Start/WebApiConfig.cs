using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RehabMakerAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Конфигурация и службы веб-API
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling
            = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            // Маршруты веб-API
            config.MapHttpAttributeRoutes();

           
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "GetParaParams",
                routeTemplate: "api/{controller}/{id}/{simbol}/{ugo}"
            );
            config.Routes.MapHttpRoute(
                name: "GetSetDevice",
                routeTemplate: "api/{controller}/{number}/{speed}"
            );


        }
    }
}
