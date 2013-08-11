namespace Peruka.Web.Modules.HelloWorld
{
    using Autofac;
    using Autofac.Integration.WebApi;

    public class HelloService : IHelloService
    {
        public string Say()
        {
            return "Hello Bitch!";
        }
    }

    public interface IHelloService
    {
        string Say();
    }

    public class HelloModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HelloService>().AsImplementedInterfaces().InstancePerApiRequest();
        }
    }
}