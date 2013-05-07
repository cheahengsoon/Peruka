namespace Peruka.Phone.Client.Presentation.TrackRoute.Details
{
    using System;
    using System.ComponentModel;

    using Knet.Phone.Client.ViewModels;

    using Peruka.Phone.Client.Core.Services;

    public class RouteDetailsViewModel : ContentScreenViewModel
    {
        private readonly RouteService _routeService;

        private bool _canStartStracking;

        private bool _canStopTracking;

        public RouteDetailsViewModel(RouteService routeService)
        {
            DisplayName = "Details";
            _routeService = routeService;

            _routeService.PropertyChanged += RouteServiceOnPropertyChanged;
        }

        private void RouteServiceOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "IsCollecting")
            {
                if (_routeService.IsCollecting == false)
                {
                    CanStartTracking = true;
                    CanStopTracking = false;
                }
                else
                {
                    CanStopTracking = true;
                    CanStartTracking = false;
                }
                return;
            }

            if (propertyChangedEventArgs.PropertyName == "IsInitialized")
            {
                if (_routeService.IsInitialized)
                {
                    CanStartTracking = true;
                }

            }
        }

        public void StartTracking()
        {
            _routeService.Start();
        }

        public void StopTracking()
        {
            _routeService.Stop();
        }

        public bool CanStartTracking
        {
            get
            {
                return _canStartStracking;
            }
            set
            {
                if (value.Equals(_canStartStracking))
                {
                    return;
                }
                _canStartStracking = value;
                NotifyOfPropertyChange(() => CanStartTracking);
            }
        }

        public bool CanStopTracking
        {
            get
            {
                return _canStopTracking;
            }
            set 
            {
                if (value.Equals(_canStopTracking))
                {
                    return;
                }
                _canStopTracking = value;
                NotifyOfPropertyChange(() => CanStopTracking);
            }
        }

        //protected override void OnActivate()
        //{
        //    _routeService.InitializeAsync();
        //}
    }
}