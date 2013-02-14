namespace Knet.Phone.Client.Caliburn
{
    using System;
    using System.IO.IsolatedStorage;

    using Autofac;

    using global::Caliburn.Micro;

    public class AutofacPhoneContainer : IPhoneContainer
    {
        private readonly IComponentContext context;

        public AutofacPhoneContainer(IComponentContext context)
        {
            this.context = context;
        }

        public event Action<object> Activated;

        public void RegisterWithAppSettings(Type service, string appSettingsKey, Type implementation)
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(appSettingsKey ?? service.FullName))
            {
                IsolatedStorageSettings.ApplicationSettings[appSettingsKey ?? service.FullName] = this.context.Resolve(implementation);
            }

            var builder = new ContainerBuilder();

            builder.Register(c =>
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains(appSettingsKey ?? service.FullName))
                {
                    return IsolatedStorageSettings.ApplicationSettings[appSettingsKey ?? service.FullName];
                }

                return c.Resolve(implementation);
            }).Named(appSettingsKey, service)
            .OnActivated(args => this.OnActivated(args.Instance));

            builder.Update(this.context.ComponentRegistry);
        }

        public void RegisterWithPhoneService(Type service, string phoneStateKey, Type implementation)
        {
            var pservice = (IPhoneService)this.GetInstance(typeof(IPhoneService), null);

            if (!pservice.State.ContainsKey(phoneStateKey ?? service.FullName))
            {
                pservice.State[phoneStateKey ?? service.FullName] = this.context.Resolve(implementation);
            }

            var builder = new ContainerBuilder();

            builder.Register(c =>
            {
                var phoneService = c.Resolve<IPhoneService>();

                if (phoneService.State.ContainsKey(phoneStateKey ?? service.FullName))
                {
                    return phoneService.State[phoneStateKey ?? service.FullName];
                }

                return c.Resolve(implementation);
            }).Named(phoneStateKey, service)
            .OnActivated(args => this.OnActivated(args.Instance));

            builder.Update(this.context.ComponentRegistry);
        }

        private object GetInstance(Type service, string key)
        {
            return string.IsNullOrEmpty(key) ? this.context.Resolve(service) : this.context.ResolveNamed(key, service);
        }

        private void OnActivated(object instance)
        {
            var handle = this.Activated;
            if (handle != null)
            {
                handle(instance);
            }
        }
    }
}