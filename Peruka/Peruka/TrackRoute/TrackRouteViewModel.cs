namespace Peruka.Phone.Client.Presentation.TrackRoute
{
    using Knet.Phone.Client.ViewModels;

    using Peruka.Phone.Client.Core.Services;

    public class TrackRouteViewModel : ScreenViewModel
    {
        private RouteService routeService;

        public TrackRouteViewModel(RouteService routeService)
        {
            this.routeService = routeService;
            this.routeService.InitializeAsync();
        }

        #region Commanding methods

        public void StartTracking()
        {
            this.routeService.Start();
        }

        public void StopTracking()
        {
            this.routeService.Stop();
        }

        #endregion
    }
}
