namespace Peruka.Phone.Client.Presentation.Login
{
    using System;
    using System.Windows.Controls;

    using Caliburn.Micro;

    using Knet.Phone.Client.ViewModels;

    using Peruka.Phone.Client.Core.Services;
    using Peruka.Phone.Client.Presentation.TrackRoute;

    public class LoginViewModel : ScreenViewModel
    {
        private readonly INavigationService _navigationService;

        private readonly PortalService _portalService;

        private readonly SettingsService _settingsService;

        private bool _canLogin;

        private string _errorMessage;

        private PasswordBox _passwordBox;

        private bool _rememberMe;

        private string _username;

        public LoginViewModel(
            INavigationService navigationService, PortalService portalService, SettingsService settingsService)
        {
            _navigationService = navigationService;
            _portalService = portalService;
            _settingsService = settingsService;

            // Set default values
            this.Username = "kajanus_demo";
            this.BusyText = "Login...";
            this.CanLogin = true;
            this.RememberMe = true;
        }

        public string ErrorMessage
        {
            get
            {
                return this._errorMessage;
            }

            set
            {
                if (value.Equals(this._errorMessage))
                {
                    return;
                }

                this._errorMessage = value;
                this.NotifyOfPropertyChange(() => this.ErrorMessage);
            }
        }

        public bool CanLogin
        {
            get
            {
                return this._canLogin;
            }

            set
            {
                if (value.Equals(this._canLogin))
                {
                    return;
                }

                this._canLogin = value;
                this.NotifyOfPropertyChange(() => this.CanLogin);
            }
        }

        public bool RememberMe
        {
            get
            {
                return this._rememberMe;
            }

            set
            {
                if (value.Equals(this._rememberMe))
                {
                    return;
                }

                this._rememberMe = value;
                this.NotifyOfPropertyChange(() => this.RememberMe);
            }
        }

        public string Username
        {
            get
            {
                return this._username;
            }

            set
            {
                if (value.Equals(this._username))
                {
                    return;
                }

                this._username = value;
                this.NotifyOfPropertyChange(() => this.Username);
            }
        }

        public void Login()
        {
            this.IsBusy = true;
            this.ErrorMessage = string.Empty;
            this.CanLogin = false;

            this.LoginAndNavigateAsync(this._passwordBox.Password);
        }

        protected override void OnActivate()
        {
            if (!string.IsNullOrEmpty(this._settingsService.Username))
            {
                this.Username = this._settingsService.Username;
            }

            // If both username and password are stored login directly  
            if (!string.IsNullOrEmpty(this.Username) && !string.IsNullOrEmpty(this._settingsService.Password))
            {
                this.LoginAndNavigateAsync(this._settingsService.Password);
            }

            base.OnActivate();
        }

        /// <summary>
        ///     Executed when the view is loaded. This violates the MVVM pattern, but is used to keep password secured.
        /// </summary>
        protected override void OnViewLoaded(object view)
        {
            this._passwordBox = (view as LoginView).PasswordBox;

            if (!string.IsNullOrEmpty(this._settingsService.Password))
            {
                this._passwordBox.Password = this._settingsService.Password;
            }

            base.OnViewLoaded(view);
        }

        private async void LoginAndNavigateAsync(string password)
        {
            try
            {
                // Generates token for the user and then naviates to the main view
                await this._portalService.InitializeAsync(this.Username, password);

                // Store username for later use.
                this._settingsService.Username = this.Username;

                // Store password if set so.
                if (this.RememberMe)
                {
                    this._settingsService.Password = password;
                }

                this.IsBusy = false;
                this._navigationService.UriFor<TrackRouteViewModel>().WithParam(p => p.BackNavSkipOne, true).Navigate();
            }
            catch (Exception exception)
            {
                this.ErrorMessage = exception.Message;
                this.IsBusy = false;
                this.CanLogin = true;
            }
        }
    }
}