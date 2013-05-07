namespace Peruka.Phone.Client.Presentation.TrackRoute
{
    using Caliburn.Micro;

    using Knet.Phone.Client.ViewModels;

    using Peruka.Phone.Client.Core.Services;
    using Peruka.Phone.Client.Presentation.Map;
    using Peruka.Phone.Client.Presentation.TrackRoute.Details;

    public class TrackRouteViewModel : ConductorViewModel
    {
        private readonly RouteDetailsViewModel _routeDetailsViewModel;

        private readonly INavigationService _navigationService;

        private RouteService _routeService;

        public TrackRouteViewModel(
            RouteService routeService, RouteDetailsViewModel routeDetailsViewModel, INavigationService navigationService)
        {
            RouteService = routeService;
            _routeDetailsViewModel = routeDetailsViewModel;
            _navigationService = navigationService;

            Items.Add(_routeDetailsViewModel);
        }

        public RouteService RouteService
        {
            get
            {
                return _routeService;
            }
            private set
            {
                if (Equals(value, _routeService))
                {
                    return;
                }
                _routeService = value;
                NotifyOfPropertyChange(() => RouteService);
            }
        }

        public bool IsCollecting
        {
            get
            {
                return _routeService.IsCollecting;
            }
        }

        public void NavigateToMap()
        {
            _navigationService.UriFor<RouteMapViewModel>().Navigate();
        }

        protected override async void OnActivate()
        {
            await _routeService.InitializeAsync();
        }
    }
}