namespace Peruka.Phone.Client.Core.Services
{
    using System.Device.Location;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Caliburn.Micro;


    // TODO : initialization handling
    // TODO : create baseclass
    public abstract class GeoService : PropertyChangedBase
    {
        #region Variables

        private GeoCoordinateWatcher coordinateWatcher;

        private bool isInitialized;

        #endregion // Variables

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoService"/> class. 
        /// </summary>
        /// <param name="settingsService">
        /// The settings service
        /// </param>
        protected GeoService(SettingsService settingsService)
        {
            this.SettingsService = settingsService;
        }

        #endregion // Constructor

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="GeoCoordinateWatcher"/>
        /// </summary>
        public GeoCoordinateWatcher CoordinateWatcher
        {
            get
            {
                return this.coordinateWatcher;
            }

            set
            {
                if (Equals(value, this.coordinateWatcher))
                {
                    return;
                }
                this.coordinateWatcher = value;
                this.NotifyOfPropertyChange(() => this.CoordinateWatcher);
            }
        }

        protected SettingsService SettingsService { get; private set; }

        #endregion // Properties

        /// <summary>
        /// Gets a value indicating whether the service is initialized.
        /// </summary>
        public bool IsInitialized
        {
            get
            {
                return this.isInitialized;
            }

            private set
            {
                if (value.Equals(this.isInitialized))
                {
                    return;
                }
                this.isInitialized = value;
                this.NotifyOfPropertyChange(() => this.IsInitialized);
            }
        }

        public async virtual Task InitializeAsync()
        {
            if (!this.SettingsService.IsLocationServiceAllowed)
            {
                return;
            }

            if (this.CoordinateWatcher != null)
            {
                return;
            }

            // TODO handle MovementTreshhold some how...
            this.CoordinateWatcher = new GeoCoordinateWatcher(this.SettingsService.PositionAccuracy) { MovementThreshold = 5 };
            this.CoordinateWatcher.StatusChanged += this.CoordinateWatcherOnStatusChanged;
            this.CoordinateWatcher.PositionChanged += this.PositionChanged;
        }

        public void Start()
        {
            this.CoordinateWatcher.Start();
            Debug.WriteLine("GeoCoordinateWather started.");
        }

        public void Stop()
        {
            this.CoordinateWatcher.Stop();
            Debug.WriteLine("GeoCoordinateWather stopped.");
        }

        /// <summary>
        /// Override when want to handle 
        /// </summary>
        /// <param name="coordinate"></param>
        public abstract void HandlePostionChanged(GeoPosition<GeoCoordinate> coordinate);

        /// <summary>
        /// <see cref="GeoCoordinateWatcher"/>s status changed handler.
        /// </summary>
        /// <param name="sender">The <see cref="GeoCoordinateWatcher"/></param>
        /// <param name="statusEventArgs">Event arguments</param>
        private void CoordinateWatcherOnStatusChanged(object sender, GeoPositionStatusChangedEventArgs statusEventArgs)
        {
            // TODO handle
            switch (statusEventArgs.Status)
            {
                case GeoPositionStatus.Disabled:
                    // The Location Service is disabled.
                    break;
                case GeoPositionStatus.Initializing:
                    // The Location Service is initializing.
                    break;
                case GeoPositionStatus.NoData:
                    // The Location Service is working, but it cannot get location data.
                    break;
                case GeoPositionStatus.Ready:
                    // The Location Service is working and is receiving location data.
                    break;
            }
        }

        /// <summary>
        /// <see cref="GeoCoordinateWatcher"/>s position changed handler.
        /// </summary>
        /// <param name="sender">
        /// The <see cref="GeoCoordinateWatcher"/>
        /// </param>
        /// <param name="e">
        /// The <see cref="GeoPositionChangedEventArgs{GeoCoordinate}"/>
        /// </param>
        private void PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (this.coordinateWatcher.Status != GeoPositionStatus.Ready)
            {
                return;
            }

            this.HandlePostionChanged(e.Position);
        }
    }
}
