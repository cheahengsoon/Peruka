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
        #region Variables

        private readonly INavigationService navigationService;
        private readonly PortalService portalService;
        private readonly SettingsService settingsService; 

        private PasswordBox passwordBox;

        private string username;
        private bool canLogin;
        private string errorMessage;
        private bool rememberMe;

        #endregion // Variables

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel"/> class. 
        /// </summary>
        /// <param name="navigationService">
        /// The navigation service
        /// </param>
        /// <param name="portalService">
        /// The portal service
        /// </param>
        /// <param name="settingsService">
        /// The settings service</param>
        public LoginViewModel(INavigationService navigationService, PortalService portalService, SettingsService settingsService)
        {
            this.navigationService = navigationService;
            this.portalService = portalService;
            this.settingsService = settingsService;

            // Set default values
            this.Username = "kajanus_demo";
            this.BusyText = "Login...";
            this.CanLogin = true;
            this.RememberMe = true;
        }

        #endregion // Constructor

        #region Properties

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username
        {
            get
            {
                return this.username;
            }

            set
            {
                if (value.Equals(this.username))
                {
                    return;
                }
                this.username = value;
                this.NotifyOfPropertyChange(() => this.Username);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user can execute Login process.
        /// </summary>
        public bool CanLogin
        {
            get
            {
                return this.canLogin;
            }

            set
            {
                if (value.Equals(this.canLogin))
                {
                    return;
                }

                this.canLogin = value;
                this.NotifyOfPropertyChange(() => this.CanLogin);
            }
        }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                return this.errorMessage;
            }

            set
            {
                if (value.Equals(this.errorMessage))
                {
                    return;
                }

                this.errorMessage = value;
                this.NotifyOfPropertyChange(() => this.ErrorMessage);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the account should be stored. 
        /// </summary>
        public bool RememberMe
        {
            get
            {
                return this.rememberMe;
            }

            set
            {
                if (value.Equals(this.rememberMe))
                {
                    return;
                }
                this.rememberMe = value;
                this.NotifyOfPropertyChange(() => this.RememberMe);
            }
        }

        #endregion // Properties

        #region Commanding methods

        /// <summary>
        /// Logs user into the application with given credentials.
        /// </summary>
        public void Login()
        {
            this.IsBusy = true;
            this.ErrorMessage = string.Empty;
            this.CanLogin = false;

            this.LoginAndNavigateAsync(this.passwordBox.Password);
        }

        #endregion // Commanding methods

        #region Lifecycle

        /// <summary>
        /// Executed when the view model is activated.
        /// </summary>
        protected override void OnActivate()
        {
            if (!string.IsNullOrEmpty(this.settingsService.Username))
            {
                this.Username = this.settingsService.Username;
            }

            // If both username and password are stored login directly  
            if (!string.IsNullOrEmpty(this.Username)
                && !string.IsNullOrEmpty(this.settingsService.Password))
            {
                this.LoginAndNavigateAsync(this.settingsService.Password);
            }

            base.OnActivate();
        }

        /// <summary>
        /// Executed when the view is loaded. This violates the MVVM pattern, but is used to keep password secured.
        /// </summary>
        /// <param name="view">The view.</param>
        protected override void OnViewLoaded(object view)
        {
            this.passwordBox = (view as LoginView).PasswordBox;
            
            if (!string.IsNullOrEmpty(this.settingsService.Password))
            {
                this.passwordBox.Password = this.settingsService.Password;
            }

            base.OnViewLoaded(view);
        }

        #endregion // Lifecycle

        #region Async methods

        /// <summary>
        /// Executes login and navigates to the next view if it was completed.
        /// </summary>
        /// <param name="password">
        /// The password.
        /// </param>
        private async void LoginAndNavigateAsync(string password)
        {
            try
            {
                // Generates token for the user and then naviates to the main view
                await this.portalService.InitializeAsync(this.Username, password);

                // Store username for later use.
                this.settingsService.Username = this.Username;

                // Store password if set so.
                if (this.RememberMe)
                {
                    this.settingsService.Password = password;
                }

                this.IsBusy = false;
                this.navigationService.UriFor<TrackRouteViewModel>().WithParam(p => p.BackNavSkipOne, true).Navigate();
            }
            catch (Exception exception)
            {
                this.ErrorMessage = exception.Message;
                this.IsBusy = false;
                this.CanLogin = true;
            }
        }

        #endregion // Async methods
    }
}
