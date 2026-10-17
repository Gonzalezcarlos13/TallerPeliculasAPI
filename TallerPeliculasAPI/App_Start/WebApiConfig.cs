using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace TallerPeliculasAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute(
                "*",
                "*",
                "GET,POST,PUT,DELETE,OPTIONS"
            );

            config.EnableCors(cors);

            config.Formatters.JsonFormatter
                .SupportedMediaTypes
                .Add(
                    new MediaTypeHeaderValue(
                        "text/html"
                    )
                );

            config.Formatters.JsonFormatter
                .SupportedMediaTypes
                .Add(
                    new MediaTypeHeaderValue(
                        "text/plain"
                    )
                );

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new
                {
                    id = RouteParameter.Optional
                }
            );
        }
    }
}