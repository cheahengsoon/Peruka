namespace Peruka.Phone.Client.Core.Services
{
    using System.Device.Location;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Caliburn.Micro;

    public abstract class GeoService : PropertyChangedBase
    {
        private GeoCoordinateWatcher _coordinateWatcher;

        private bool _isInitialized;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GeoService" /> class.
        /// </summary>
        /// <param name="settingsService">
        ///     The settings service
        /// </param>
        protected GeoService(SettingsService settingsService)
        {
            SettingsService = settingsService;
        }

        /// <summary>
        ///     Gets or sets the <see cref="GeoCoordinateWatcher" />
        /// </summary>
        public GeoCoordinateWatcher CoordinateWatcher
        {
            get
            {
                return _coordinateWatcher;
            }

            set
            {
                if (Equals(value, _coordinateWatcher))
                {
                    return;
                }
                _coordinateWatcher = value;
                NotifyOfPropertyChange(() => CoordinateWatcher);
            }
        }

        protected SettingsService SettingsService { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether the service is initialized.
        /// </summary>
        public bool IsInitialized
        {
            get
            {
                return this._isInitialized;
            }

            private set
            {
                if (value.Equals(_isInitialized))
                {
                    return;
                }
                _isInitialized = value;
                NotifyOfPropertyChange(() => IsInitialized);
            }
        }

        public virtual async Task InitializeAsync()
        {
            if (!SettingsService.IsLocationServiceAllowed)
            {
                return;
            }

            if (CoordinateWatcher != null)
            {
                return;
            }

            // TODO handle MovementTreshhold some how...
            CoordinateWatcher = new GeoCoordinateWatcher(SettingsService.PositionAccuracy) { MovementThreshold = 5 };
            CoordinateWatcher.StatusChanged += CoordinateWatcherOnStatusChanged;
            CoordinateWatcher.PositionChanged += PositionChanged;

            IsInitialized = true;
        }

        public void Start()
        {
            CoordinateWatcher.Start();
            Debug.WriteLine("GeoCoordinateWather started.");
        }

        public void Stop()
        {
            CoordinateWatcher.Stop();
            Debug.WriteLine("GeoCoordinateWather stopped.");
        }

        /// <summary>
        ///     Override when want to handle
        /// </summary>
        /// <param name="coordinate"></param>
        public abstract void HandlePostionChanged(GeoPosition<GeoCoordinate> coordinate);

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

        private void PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (_coordinateWatcher.Status != GeoPositionStatus.Ready)
            {
                return;
            }

            HandlePostionChanged(e.Position);
        }
    }
}