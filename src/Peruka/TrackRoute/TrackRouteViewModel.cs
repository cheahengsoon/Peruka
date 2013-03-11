namespace Peruka.Phone.Client.Presentation.TrackRoute
{
    using Knet.Phone.Client.ViewModels;

    using Peruka.Phone.Client.Core.Services;

    public class TrackRouteViewModel : ScreenViewModel
    {
        private readonly RouteService _routeService;

        public TrackRouteViewModel(RouteService routeService)
        {
            _routeService = routeService;
            _routeService.InitializeAsync();
        }

        #region Commanding methods

        public void StartTracking()
        {
            _routeService.Start();
        }

        public void StopTracking()
        {
            _routeService.Stop();
        }

        #endregion
    }
}