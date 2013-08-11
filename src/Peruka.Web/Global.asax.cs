namespace Peruka.Web
{
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using App_Start;
    using Autofac;
    using Autofac.Integration.WebApi;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            var containerBuilder = new ContainerBuilder();
            var configuration = GlobalConfiguration.Configuration;

            AutofacConfig.Register(containerBuilder, configuration);
            WebApiConfig.Register(configuration, containerBuilder);

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(containerBuilder.Build());
        }
    }
}