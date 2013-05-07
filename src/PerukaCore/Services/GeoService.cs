namespace Peruka.Phone.Client.Core.Services
{
    using System.Device.Location;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Caliburn.Micro;

    public abstract class GeoService : PropertyChangedBase
    {
        public enum GeoServiceStatus
        {
            NotSet,

            Started,

            Stopped
        }

        private GeoCoordinateWatcher _coordinateWatcher;

        private bool _isInitialized;

        private GeoServiceStatus _serviceStatus;

        private bool _isCollecting;

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

        /// <summary>
        /// Gets the status of the service.
        /// </summary>
        public GeoServiceStatus ServiceStatus
        {
            get
            {
                return _serviceStatus;
            }
            protected set
            {
                if (value == _serviceStatus)
                {
                    return;
                }
                _serviceStatus = value;
                NotifyOfPropertyChange(() => ServiceStatus);
            }
        }

        /// <summary>
        ///     Gets the value indicating whether the service is collection GPS information.
        /// </summary>
        public bool IsCollecting
        {
            get
            {
                return _isCollecting;
            }
            set
            {
                if (value.Equals(_isCollecting))
                {
                    return;
                }
                _isCollecting = value;
                NotifyOfPropertyChange(() => IsCollecting);
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
            ServiceStatus = GeoServiceStatus.Started;
            Debug.WriteLine("GeoCoordinateWather started.");
        }

        public void Stop()
        {
            CoordinateWatcher.Stop();
            ServiceStatus = GeoServiceStatus.Stopped;
            IsCollecting = false;
            Debug.WriteLine("GeoCoordinateWather stopped.");
        }

        /// <summary>
        ///     Override when to handle position changed events.
        /// </summary>
        /// <param name="coordinate"></param>
        public abstract void HandlePositionChanged(GeoPosition<GeoCoordinate> coordinate);

        private void CoordinateWatcherOnStatusChanged(object sender, GeoPositionStatusChangedEventArgs statusEventArgs)
        {
            // TODO handle
            switch (statusEventArgs.Status)
            {
                case GeoPositionStatus.Disabled:
                    // The Location Service is disabled.
                    if (IsCollecting)
                    {
                        IsCollecting = false;
                    }
                    break;
                case GeoPositionStatus.Initializing:
                    // The Location Service is initializing.
                    if (IsCollecting)
                    {
                        IsCollecting = false;
                    }
                    break;
                case GeoPositionStatus.NoData:
                    // The Location Service is working, but it cannot get location data.
                    if (IsCollecting)
                    {
                        IsCollecting = false;
                    }
                    break;
                case GeoPositionStatus.Ready:
                    // The Location Service is working and is receiving location data.
                    IsCollecting = true;
                    break;
            }
        }

        private void PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (ServiceStatus != GeoServiceStatus.Started || IsCollecting == false)
            {
                return;
            }

            HandlePositionChanged(e.Position);
        }
    }
}