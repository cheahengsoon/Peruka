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
        private readonly INavigationService _navigationService;

        private readonly SettingsService _settingsService;

        private readonly PortalService _portalService;

        public SplashViewModel(
            INavigationService navigationService, SettingsService settingsService, PortalService portalService)
        {
            _navigationService = navigationService;
            _settingsService = settingsService;
            _portalService = portalService;
        }

        protected override void OnActivate()
        {
            // If both username and password are stored login directly  
            if (!string.IsNullOrEmpty(_settingsService.Username) && !string.IsNullOrEmpty(_settingsService.Password))
            {
                Debug.WriteLine("Username and password found from settings store.");

                // Login and navigate to the 
                LoginAndNavigateAsync();
            }
            else
            {
                Debug.WriteLine("Username and password not found from settings store.");
                Debug.WriteLine("Navigating to LoginView");

                // Navigate to the TrackRoute 
                _navigationService.UriFor<LoginViewModel>().WithParam(p => p.BackNavSkipOne, true).Navigate();
            }

            base.OnActivate();
        }

        private async void LoginAndNavigateAsync()
        {
            try
            {
                IsBusy = true;
                BusyText = "Login...";

                Debug.WriteLine("Logining...");

                // Generates token for the user and then naviates to the main view
                await _portalService.InitializeAsync(_settingsService.Username, _settingsService.Password);

                Debug.WriteLine("Login completed.");

                Debug.WriteLine("Navigating to the TrackRouteView");
                _navigationService.UriFor<TrackRouteViewModel>().WithParam(p => p.BackNavSkipOne, true).Navigate();
                IsBusy = false;
            }
            catch (Exception exception)
            {
                // TODO : indicate exception somehow here or in the login page.
                Debug.WriteLine("Login failed : {0}", exception);

                _navigationService.UriFor<LoginViewModel>().WithParam(wm => wm.BackNavSkipOne, true);
                IsBusy = false;
            }
        }
    }
}