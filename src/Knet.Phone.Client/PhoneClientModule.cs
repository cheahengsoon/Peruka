namespace Knet.Phone.Client
{
    using Autofac;

    using Caliburn;

    using global::Caliburn.Micro;

    /// <summary>
    /// Phone client services
    /// </summary>
    public class PhoneClientModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context => new AutofacPhoneContainer(context)).As<IPhoneContainer>().SingleInstance();
        }
    }
}