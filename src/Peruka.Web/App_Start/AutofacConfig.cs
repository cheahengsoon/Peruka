namespace Peruka.Web.App_Start
{
    using System.Web.Http;
    using Autofac;

    public class AutofacConfig
    {
        public static void Register(ContainerBuilder container, HttpConfiguration configuration)
        {
            container.RegisterAssemblyModules(typeof(AutofacConfig).Assembly);
        }
    }
}