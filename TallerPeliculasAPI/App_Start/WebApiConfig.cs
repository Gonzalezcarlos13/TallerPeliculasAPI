using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
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

            config.MessageHandlers.Add(
                new OptionsRequestHandler()
            );

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

        private class OptionsRequestHandler : DelegatingHandler
        {
            protected override async Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                CancellationToken cancellationToken
            )
            {
                if (request.Method == HttpMethod.Options)
                {
                    var response =
                        request.CreateResponse(
                            HttpStatusCode.OK
                        );

                    response.Headers.TryAddWithoutValidation(
                        "Access-Control-Allow-Origin",
                        "*"
                    );

                    response.Headers.TryAddWithoutValidation(
                        "Access-Control-Allow-Headers",
                        "Content-Type, Accept, Authorization, X-Requested-With"
                    );

                    response.Headers.TryAddWithoutValidation(
                        "Access-Control-Allow-Methods",
                        "GET, POST, PUT, DELETE, OPTIONS"
                    );

                    return response;
                }

                return await base.SendAsync(
                    request,
                    cancellationToken
                );
            }
        }
    }
}
