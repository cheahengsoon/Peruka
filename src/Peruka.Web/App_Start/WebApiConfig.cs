namespace Peruka.Web
{
    using System.Web.Http;
    using Autofac;
    using Autofac.Integration.WebApi;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config, ContainerBuilder container)
        {
            // configure web api - autofac integration
            container.RegisterWebApiModelBinderProvider();
            container.RegisterWebApiFilterProvider(config);
            container.RegisterWebApiModelBinders(typeof(WebApiApplication).Assembly);
            container.RegisterApiControllers(typeof(WebApiApplication).Assembly);

            // register default route
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();
        }
    }
}