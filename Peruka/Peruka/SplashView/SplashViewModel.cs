namespace Peruka.Phone.Client.Presentation.SplashView
{
    using System;
    using System.Diagnostics;

    using Caliburn.Micro;

    using Knet.Phone.Client.ViewModels;

    using Peruka.Phone.Client.Core.Services;
    using Peruka.Phone.Client.Presentation.Login;
    using Peruka.Phone.Client.Presentation.TrackRoute;

    public class SplashViewModel : ScreenViewModel
    {
        #region Variables

        private readonly INavigationService navigationService;

        private readonly SettingsService settingsService;

        private readonly PortalService portalService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SplashViewModel"/> class. 
        /// </summary>
        /// <param name="navigationService">
        /// The navigation service
        /// </param>
        /// <param name="settingsService">
        /// The settings service
        /// </param>
        /// <param name="portalService">
        /// The portal service
        /// </param>
        public SplashViewModel(INavigationService navigationService, SettingsService settingsService, PortalService portalService)
        {
            this.navigationService = navigationService;
            this.settingsService = settingsService;
            this.portalService = portalService;
        }

        #endregion // Constructor

        #region Lifecycle events

        /// <summary>
        /// Executed when the view model is activated.
        /// </summary>
        protected override void OnActivate()
        {
            // If both username and password are stored login directly  
            if (!string.IsNullOrEmpty(this.settingsService.Username)
                && !string.IsNullOrEmpty(this.settingsService.Password))
            {
                Debug.WriteLine("Username and password found from settings store.");

                // Login and navigate to the 
                this.LoginAndNavigateAsync();
            }
            else
            {
                Debug.WriteLine("Username and password not found from settings store.");
                Debug.WriteLine("Navigating to LoginView");
                
                // Navigate to the TrackRoute 
                this.navigationService.UriFor<LoginViewModel>().WithParam(p => p.BackNavSkipOne, true).Navigate();
            }

            base.OnActivate();
        }

        #endregion // Lifecycle events

        #region Async methods

        /// <summary>
        /// Executes login and navigates to the next view.
        /// </summary>
        private async void LoginAndNavigateAsync()
        {
            try
            {
                this.IsBusy = true;
                this.BusyText = "Login...";

                Debug.WriteLine("Logining...");

                // Generates token for the user and then naviates to the main view
                await this.portalService.InitializeAsync(this.settingsService.Username, this.settingsService.Password);
                
                Debug.WriteLine("Login completed.");

                Debug.WriteLine("Navigating to the TrackRouteView");
                this.navigationService.UriFor<TrackRouteViewModel>().WithParam(p => p.BackNavSkipOne, true).Navigate();
                this.IsBusy = false;
            }
            catch (Exception exception)
            {
                // TODO : indicate exception somehow here or in the login page.
                Debug.WriteLine("Login failed : {0}", exception);
                
                this.navigationService.UriFor<LoginViewModel>().WithParam(wm => wm.BackNavSkipOne, true);
                this.IsBusy = false;
            }
        }

        #endregion // Async methods
    }
}
