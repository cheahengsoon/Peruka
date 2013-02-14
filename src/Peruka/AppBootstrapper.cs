namespace Peruka.Phone.Client.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;

    using Autofac;

    using Caliburn.Micro;
    using Knet.Phone.Client;
    using Microsoft.Phone.Controls;

    using Peruka.Phone.Client.Core.Services;

    /// <summary>
    /// Bootstrapper for the application. This class constructs and hooks phone application and its services. 
    /// </summary>
    public class AppBootstrapper : PhoneBootstrapper
    {
        private IContainer container;

        /// <summary>
        /// Configures the application.
        /// This is the place where all hooking is done with IoC and different phone services.
        /// </summary>
        protected override void Configure()
        {
            // configure container
            var builder = new ContainerBuilder();

            // register view models from this assembly
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(type => type.Name.EndsWith("ViewModel"))
                .AsSelf()
                .InstancePerDependency();

            // Register Portal Service
            builder.Register(c => new PortalService()).SingleInstance();

            // Register Settings Service
            builder.Register(c => new SettingsService()).SingleInstance();

            // Register Settings Service
            builder.Register(c => new RouteService(container.Resolve<SettingsService>())).SingleInstance();

            
            //// register view models from this assembly
            //builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
            //    .Where(type => type.Name.EndsWith("Service"))
            //    .AsSelf()
            //    .SingleInstance();
            
            // Register phone services
            var caliburnAssembly = typeof(IStorageMechanism).Assembly;

            // Register IStorageMechanism implementors
            builder.RegisterAssemblyTypes(caliburnAssembly)
                .Where(
                    type => typeof(IStorageMechanism).IsAssignableFrom(type)
                           && !type.IsAbstract
                           && !type.IsInterface)
               .As<IStorageMechanism>()
               .InstancePerLifetimeScope();

            // Register IStorageHandler implementors
            builder.RegisterAssemblyTypes(caliburnAssembly)
               .Where(
                     type => typeof(IStorageHandler).IsAssignableFrom(type)
                             && !type.IsAbstract
                             && !type.IsInterface)
                 .As<IStorageHandler>()
                 .InstancePerLifetimeScope();

            // register knet services
            builder.RegisterAssemblyModules(typeof (PhoneClientModule).Assembly);

            // Register the singletons
            var frameAdapter = new FrameAdapter(this.RootFrame);
            var phoneServices = new PhoneApplicationServiceAdapter(this.RootFrame);

            builder.RegisterInstance(frameAdapter).As<INavigationService>();
            builder.RegisterInstance(phoneServices).As<IPhoneService>();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<WindowManager>().As<IWindowManager>().SingleInstance();
            builder.RegisterType<StorageCoordinator>().AsSelf().SingleInstance();
            builder.RegisterType<TaskController>().AsSelf().SingleInstance();

            // Build the container
            this.container = builder.Build();

            // Start services
            this.container.Resolve<StorageCoordinator>().Start();
            this.container.Resolve<TaskController>().Start();

            AddCustomConventions();

            this.RootFrame.Navigated += this.RootFrameNavigated;
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return this.container.Resolve(typeof(IEnumerable<>).MakeGenericType(service)) as IEnumerable<object>;
        }

        protected override object GetInstance(Type service, string key)
        {
            object instance;
            if (string.IsNullOrEmpty(key))
            {
                if (this.container.TryResolve(service, out instance))
                {
                    return instance;
                }
            }
            else
            {
                if (this.container.TryResolveNamed(key, service, out instance))
                {
                    return instance;
                }
            }

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", key ?? service.Name));
        }

        protected override void BuildUp(object instance)
        {
            this.container.InjectUnsetProperties(instance);
        }

        protected override void OnUnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            #warning TODO Exception handling

            #if DEBUG

                MessageBox.Show(e.ExceptionObject.ToString());
                Debug.WriteLine("Error : {0}", e.ExceptionObject);

            #endif
        }

        /// <summary>
        /// Registering custom convetions for the Caliburn.Micro bindings.
        /// </summary>
        private static void AddCustomConventions()
        {
            ConventionManager.AddElementConvention<Pivot>(Pivot.ItemsSourceProperty, "SelectedItem", "SelectionChanged").ApplyBinding =
               (viewModelType, path, property, element, convention) =>
               {
                   if (ConventionManager
                       .GetElementConvention(typeof(ItemsControl))
                       .ApplyBinding(viewModelType, path, property, element, convention))
                   {
                       ConventionManager
                           .ConfigureSelectedItem(element, Pivot.SelectedItemProperty, viewModelType, path);
                       ConventionManager
                           .ApplyHeaderTemplate(element, Pivot.HeaderTemplateProperty, null, viewModelType);
                       return true;
                   }

                   return false;
               };

           ConventionManager.AddElementConvention<Panorama>(Panorama.ItemsSourceProperty, "SelectedItem", "SelectionChanged").ApplyBinding =
                (viewModelType, path, property, element, convention) =>
                {
                    if (ConventionManager
                        .GetElementConvention(typeof(ItemsControl))
                        .ApplyBinding(viewModelType, path, property, element, convention))
                    {
                        ConventionManager
                            .ConfigureSelectedItem(element, Panorama.SelectedItemProperty, viewModelType, path);
                        ConventionManager
                            .ApplyHeaderTemplate(element, Panorama.HeaderTemplateProperty, null, viewModelType);
                        return true;
                    }

                    return false;
                };
        }

        private void RootFrameNavigated(object sender, NavigationEventArgs e)
        {
            // var navService = GetInstance(typeof(INavigationService), null) as INavigationService;
            if (e.NavigationMode == NavigationMode.New && e.Uri.ToString().Contains("BackNavSkipOne=True"))
            {
                Debug.WriteLine("Removing back navigation entry.");
                RootFrame.RemoveBackEntry();
            }
        }
    }
}