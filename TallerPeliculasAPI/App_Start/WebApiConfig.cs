using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace TallerPeliculasAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // 1. HABILITAR CORS GLOBALMENTE
            // Esto es el "permiso" para que React pueda hablar con tu API
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // 2. FORZAR FORMATO JSON
            // Esto evita el error de serialización y la pantalla de error XML
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));

            // 3. Rutas existentes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}